using Amazon.CDK;
using Amazon.CDK.AWS.EC2;
using Amazon.CDK.AWS.ECR;
using Amazon.CDK.AWS.S3;
using Amazon.CDK.AWS.SQS;
using Aws.Meetup.Cdk.Configuration;
using Constructs;

namespace Aws.Meetup.Cdk.Stacks;

public class SharedStack : Stack
{
    public SharedStack(Construct scope, string id, Config config, IStackProps props) : base(scope, id, props)
    {
        // S3 bucket
        var bucket = new Bucket(this, "meetup-bucket", new BucketProps
        {
            BucketName = $"{config.Prefix}-bucket",
            Versioned = true
        });

        // SQS queue
        var queue = new Queue(this, "meetup-queue", new QueueProps
        {
            QueueName = $"{config.Prefix}-queue",
            VisibilityTimeout = Duration.Seconds(30)
        });

        // ECR Repository for docker images
        new Repository(this, "meetup-ecr-repository", new RepositoryProps
        {
            RepositoryName = $"{config.Prefix}-repository",
            RemovalPolicy = RemovalPolicy.DESTROY
        });

        new Vpc(this, "meetup-vpc", new VpcProps
        {
            VpcName = $"{config.Prefix}-vpc",
            MaxAzs = 2,
            SubnetConfiguration =
            [
                new SubnetConfiguration
                {
                    SubnetType = SubnetType.PRIVATE_WITH_EGRESS,
                    Name = "Private"
                },
                new SubnetConfiguration
                {
                    SubnetType = SubnetType.PUBLIC,
                    Name = "Public"
                }
            ]
        });

        // Exports
        new CfnOutput(this, "meetup-bucket-arn-output", new CfnOutputProps
        {
            Value = bucket.BucketArn,
            ExportName = "meetup-bucket-arn"
        });

        new CfnOutput(this, "meetup-queue-arn-output", new CfnOutputProps
        {
            Value = queue.QueueArn,
            ExportName = "meetup-queue-arn"
        });
    }
}