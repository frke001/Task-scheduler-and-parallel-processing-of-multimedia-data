using DokanNet;
using DokanNet.Logging;
using KorisnickiFajlSistem;

char driveLetter = 'E'; 

using (ConsoleLogger consoleLogger = new("[UserSpaceFileSystem]"))
using (Dokan dokan = new(consoleLogger))
{
    string mountPoint = $"{driveLetter}:\\"; // variable insertion
    UserspaceFileSistem myFileSystem = new();
    DokanInstanceBuilder dokanInstanceBuilder = new DokanInstanceBuilder(dokan)
        .ConfigureLogger(() => consoleLogger)
        .ConfigureOptions(options =>
        {
            options.Options = DokanOptions.DebugMode;
            options.MountPoint = mountPoint;
        });
    using DokanInstance dokanInstance = dokanInstanceBuilder.Build(myFileSystem);
    Console.ReadLine();
    //Console.WriteLine(File.ReadAllText("E:\\input\\Momci.txt"));
    Console.ReadLine();
}