#tool "nuget:?package=NUnit.ConsoleRunner&version=3.9.0"

//-------------------------------------------------------------

private void RunTestsUsingNUnit(string projectName, string testTargetFramework, string testResultsDirectory)
{
    var testFile = string.Format("{0}/{1}/{2}.dll", GetProjectOutputDirectory(projectName), testTargetFramework, projectName);
    var resultsFile = string.Format("{0}/testresults.xml", testResultsDirectory);

    NUnit3(testFile, new NUnit3Settings
    {
        Results = new NUnit3Result[] 
        {
            new NUnit3Result
            {
                FileName = resultsFile
            }
        },
        NoHeader = true,
        NoColor = true,
        //Work = testResultsDirectory
    });

    if (!FileExists(resultsFile))
    {
        Error(string.Format("Expected results file '{0}' does not exist", resultsFile));
    }
}