using System.Net.Http.Json;
using Amazon.CDK.AWS.EC2;
using Amazon.CDK.AWS.ECR;
using Amazon.CDK.AWS.ECS;
using Amazon.CDK.AWS.ElasticLoadBalancingV2;
using Amazon.CDK.AWS.IAM;
using Aws.Meetup.Cdk.Configuration;
using Constructs;
using ApplicationLoadBalancerProps = Amazon.CDK.AWS.ElasticLoadBalancingV2.ApplicationLoadBalancerProps;
using HealthCheck = Amazon.CDK.AWS.ElasticLoadBalancingV2.HealthCheck;

namespace Aws.Meetup.Cdk.Constructs;

public class FargateConstruct : Construct
{
    public FargateConstruct(Construct scope,
        string id,
        string bucketArn,
        string queueArn,
        Config config,
        IRepository repository,
        IVpc vpc,
        ICluster cluster) :
        base(scope, id)
    {
        var sqsPolicyStatement = new PolicyStatement(new PolicyStatementProps
        {
            Actions = ["sqs:SendMessage"],
            Resources = [queueArn]
        });

        var s3PolicyStatement = new PolicyStatement(new PolicyStatementProps
        {
            Actions = ["s3:GetObject"],
            Resources = [$"{bucketArn}/*"]
        });

        var taskRole = new Role(this, "meetup-fargate-task-role", new RoleProps
        {
            AssumedBy = new ServicePrincipal("ecs-tasks.amazonaws.com"),
            RoleName = $"{config.Prefix}-task-role"
        });

        taskRole.AddToPolicy(sqsPolicyStatement);
        taskRole.AddToPolicy(s3PolicyStatement);

        var taskDefinition = new FargateTaskDefinition(this, "meetup-task-definition", new FargateTaskDefinitionProps
        {
            TaskRole = taskRole,
            RuntimePlatform = new RuntimePlatform
            {
                CpuArchitecture = CpuArchitecture.ARM64
            }
        });

        var container = taskDefinition.AddContainer("meetup-container", new ContainerDefinitionOptions
        {
            ContainerName = $"{config.Prefix}-container",
            Image = ContainerImage.FromEcrRepository(repository, "latest"),
            Cpu = 256,
            MemoryLimitMiB = 512,
            Environment = config.WebApi.EnvironmentVariables
        });

        container.AddPortMappings(new PortMapping
        {
            ContainerPort = 8080
        });

        var loadBalancerSecurityGroup = new SecurityGroup(this, "meetup-alb-security-group", new SecurityGroupProps
        {
            Vpc = vpc
        });

        // Dirty trick to only allow my IP for demo purposes. Should not be done this way in real environments.
        using var httpClient = new HttpClient();
        var myIp = httpClient.GetFromJsonAsync<MyIp>("https://api.ipify.org/?format=json", JsonOptions.Default).Result!;
        
        loadBalancerSecurityGroup.AddIngressRule(Peer.Ipv4($"{myIp.Ip}/32"), Port.Tcp(80));

        var loadBalancer = new ApplicationLoadBalancer(this, "meetup-load-balancer", new ApplicationLoadBalancerProps
        {
            LoadBalancerName = $"{config.Prefix}-api",
            Vpc = vpc,
            SecurityGroup = loadBalancerSecurityGroup,
            InternetFacing = true
        });

        var listener = loadBalancer.AddListener("meetup-listener", new BaseApplicationListenerProps
        {
            Port = 80,
            Protocol = ApplicationProtocol.HTTP,
            // Set this to true if you want to allow anyone accessing your load balancer.
            // This is only set to false in order to prevent the default 0.0.0.0/0 rule from being created
            Open = false
        });

        var serviceSecurityGroup = new SecurityGroup(this, "ServiceSecurityGroup", new SecurityGroupProps
        {
            Vpc = vpc
        });

        serviceSecurityGroup.AddIngressRule(loadBalancerSecurityGroup, Port.Tcp(8080),
            "Allow traffic from load balancer");

        var service = new FargateService(this, "meetup-fargate-service", new FargateServiceProps
        {
            ServiceName = $"{config.Prefix}-service",
            Cluster = cluster,
            TaskDefinition = taskDefinition,
            VpcSubnets = new SubnetSelection {SubnetType = SubnetType.PRIVATE_WITH_EGRESS},
            SecurityGroups = [serviceSecurityGroup]
        });

        service.RegisterLoadBalancerTargets(new EcsTarget
        {
            ContainerName = $"{config.Prefix}-container",
            ContainerPort = 8080,
            NewTargetGroupId = "ECS",
            Listener = ListenerConfig.ApplicationListener(listener,
                new AddApplicationTargetsProps
                {
                    Port = 8080,
                    Protocol = ApplicationProtocol.HTTP,
                    HealthCheck = new HealthCheck
                    {
                        Port = "8080",
                        Path = "/swagger/index.html"
                    }
                })
        });
    }

    private record MyIp(string Ip);
}