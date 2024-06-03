using Amazon.CDK;
using Aws.Meetup.Cdk.Configuration;
using Aws.Meetup.Cdk.Stacks;

var app = new App();
var environment = app.Node.TryGetContext("env")?.ToString() ?? "dev";
var config = Config.ParseFrom(app, environment);

var env = new Amazon.CDK.Environment
{
    Account = config.Account,
    Region = config.Region
};

var sharedStack = new SharedStack(app, "shared", config, new StackProps {Env = env});
var webApiStack = new WebApiStack(app, "webapi", config, new StackProps {Env = env});
var lambdaStack = new LambdaStack(app, "lambda", config, new StackProps {Env = env});

webApiStack.AddDependency(sharedStack);
lambdaStack.AddDependency(sharedStack);

app.Synth();