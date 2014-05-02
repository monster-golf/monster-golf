param( 	[string]$u = "", [string]$p = "" )
if ("$u" -eq "" -or "$p" -eq "") {
    "Please set -u and -p"
    exit;
}
$startpath = get-location
$outpath = "/monstergolf"
$dir = "../monster-golf-web/" + $outpath
 
#ftp server 
set-location $dir
$location = get-location
$ftp = "ftp://97.74.215.204"+$outpath 
$webclient = New-Object System.Net.WebClient 
$webclient.Credentials = New-Object System.Net.NetworkCredential($u,$p)  
$files = Get-ChildItem -Recurse | Where-Object  {
   $_.LastWriteTime.ToShortDateString() -eq (Get-Date).ToShortDateString() 
}

foreach($file in $files)
{
    $directory = "";
    $source = $file.DirectoryName + "\" + $file;
    if ($file.DirectoryName.Length -gt 0)
    {
        $directory = $file.DirectoryName.Replace($location,"")
    }
    $directory += "/";
    if ("$file" -eq "Web.config" -or "$file" -eq "PrecompiledApp.config" -or "$file" -eq "PublishProfiles" -or "$file" -eq "website.publishproj" -or "$file" -eq "monsterscoring.pubxml" -or "$file" -eq "App_Data" -or "$file" -eq "bin") {
        "Skipping $file"
    } else {
        $ftp_command = $ftp + $directory + $file
        $uri = New-Object System.Uri($ftp_command)
        "updating $uri from $source" 
        #$webclient.UploadFile($uri, $source)
    }
}

set-location $startpath