using Amazon.CDK;
using Amazon.CDK.AWS.EC2;
using Amazon.CDK.AWS.ECR;
using Amazon.CDK.AWS.ECS;
using Amazon.CDK.AWS.ECS.Patterns;
using Aws.Meetup.Cdk.Configuration;
using Aws.Meetup.Cdk.Constructs;
using Constructs;

namespace Aws.Meetup.Cdk.Stacks;

/// <summary>
///     A CloudFormation stack containing everything required to run & deploy the Aws.Meetup.WebApi in ECS Fargate
///     including access to SQS and S3
/// </summary>
/// <remarks>
///     You could also use the <see cref="ApplicationLoadBalancedFargateService" /> L3 construct here.
/// </remarks>
public class WebApiStack : Stack
{
    internal WebApiStack(Construct scope, string id, Config config, IStackProps props) : base(scope, id, props)
    {
        // Imports
        var bucketArn = Fn.ImportValue("meetup-bucket-arn");
        var queueArn = Fn.ImportValue("meetup-queue-arn");
        
        var repository = Repository.FromRepositoryName(this, "meetup-repository", $"{config.Prefix}-repository");

        var vpc = Vpc.FromLookup(this, "meetup-vpc", new VpcLookupOptions
        {
            VpcName = $"{config.Prefix}-vpc"
        });

        var cluster = new Cluster(this, "meetup-cluster", new ClusterProps
        {
            ClusterName = $"{config.Prefix}-cluster",
            Vpc = vpc
        });

        new FargateConstruct(this, "meetup-fargate-task", bucketArn, queueArn, config, repository, vpc, cluster);
    }
}