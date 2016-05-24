// include Fake libs
#r "./packages/FAKE/tools/FakeLib.dll"

open Fake

// Directories
let buildDir  = "./build/"
let deployDir = "./deploy/"


// Filesets
let appReferences  =
    !! "/**/*.csproj"
      ++ "/**/*.fsproj"

// version info
let version = "0.1"  // or retrieve from CI server

// Targets
Target "Clean" (fun _ ->
    CleanDirs [buildDir; deployDir]
)

Target "Build" (fun _ ->
    // compile all projects below src/app/
    MSBuildDebug buildDir "Build" appReferences
        |> Log "AppBuild-Output: "
)

Target "Deploy" (fun _ ->
    !! (buildDir + "/**/*.*")
        -- "*.zip"
        |> Zip buildDir (deployDir + "ApplicationName." + version + ".zip")
)

Target "Test" (fun _ ->
  ExecProcess
    (fun info ->
     info.FileName <- (buildDir @@ "AStar.exe")
    ) (System.TimeSpan.FromMinutes 60.)
  |> ignore
)

// Build order
"Clean"
  ==> "Build"
  ==> "Deploy"

"Clean"
  ==> "Build"
  ==> "Test"

// start build
RunTargetOrDefault "Build"
