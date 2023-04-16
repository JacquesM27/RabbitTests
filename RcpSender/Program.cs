// See https://aka.ms/new-console-template for more information
using RpcSender;

Console.WriteLine("Hello, Rpc client!");
while (true)
{
    Console.WriteLine("Type some number:");
    string number = Console.ReadLine();
    await InvokeAsync(number);
}


static async Task InvokeAsync(string number)
{
    using var rpcClient = new RpcClient();
    Console.WriteLine($"Requesting is_even({number})");
    var response = await rpcClient.CallAsync( number );
    Console.WriteLine($"Respose:{response}");
}

