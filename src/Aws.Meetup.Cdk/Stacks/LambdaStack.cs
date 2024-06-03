using Amazon.CDK;
using Amazon.CDK.AWS.EC2;
using Amazon.CDK.AWS.IAM;
using Amazon.CDK.AWS.Lambda;
using Amazon.CDK.AWS.Lambda.EventSources;
using Amazon.CDK.AWS.Logs;
using Amazon.CDK.AWS.SQS;
using Aws.Meetup.Cdk.Configuration;
using Constructs;

namespace Aws.Meetup.Cdk.Stacks;

/// <summary>
///     A CloudFormation stack containing everything required to run the Aws.Meetup.Lambda including access to SQS and S3
/// </summary>
public class LambdaStack : Stack
{
    internal LambdaStack(Construct scope, string id, Config config, IStackProps props) : base(scope, id, props)
    {
        // Imports
        var bucketArn = Fn.ImportValue("meetup-bucket-arn");
        var queueArn = Fn.ImportValue("meetup-queue-arn");

        // policy statements with least-privilege access
        var sqsPolicyStatement = new PolicyStatement(new PolicyStatementProps
        {
            Actions =
            [
                "sqs:ReceiveMessage",
                "sqs:DeleteMessage",
                "sqs:GetQueueAttributes"
            ],
            Resources = [queueArn]
        });

        var s3PolicyStatement = new PolicyStatement(new PolicyStatementProps
        {
            Actions = ["s3:PutObject"],
            Resources = [$"{bucketArn}/*"]
        });

        // Create the lambda role and attach the policy statements
        var role = new Role(this, "meetup-lambda-role", new RoleProps
        {
            AssumedBy = new ServicePrincipal("lambda.amazonaws.com")
        });

        role.AddToPolicy(sqsPolicyStatement);
        role.AddToPolicy(s3PolicyStatement);
        
        role.AddManagedPolicy(ManagedPolicy.FromAwsManagedPolicyName("service-role/AWSLambdaBasicExecutionRole"));
        role.AddManagedPolicy(ManagedPolicy.FromAwsManagedPolicyName("service-role/AWSLambdaVPCAccessExecutionRole"));
        
        var vpc = Vpc.FromLookup(this, "meetup-vpc", new VpcLookupOptions
        {
            VpcName = $"{config.Prefix}-vpc"
        });

        // Lambda function 
        var function = new Function(this, "meetup-lambda", new FunctionProps
        {
            Runtime = Runtime.DOTNET_8,
            MemorySize = 512,
            LogRetention = RetentionDays.ONE_DAY,
            Handler = config.Lambda.Handler,
            Code = Code.FromAsset(config.Lambda.CodePath),
            Architecture = Architecture.ARM_64,
            Vpc = vpc,
            Role = role,
            VpcSubnets = new SubnetSelection
            {
                SubnetType = SubnetType.PRIVATE_WITH_EGRESS
            }
        });

        // Add the SQS event source to the Lambda function
        var queue = Queue.FromQueueArn(this, "queue", queueArn);
        function.AddEventSource(new SqsEventSource(queue));
    }
}