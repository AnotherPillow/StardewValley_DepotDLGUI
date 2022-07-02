import os
import urllib.request
import requests
import zipfile

print("""
    Automatic installer for DepotDownloader
""")

dotnet = os.popen("dotnet --list-runtimes").read()
print("Scanning for .NET runtimes...")
dotnet_installs = dotnet.split("\n")

for l in dotnet_installs:
    if "6.0" in l:
        net6_install = True

if net6_install != True:
    print("""
    Please install .NET 6.0
    You can find the download link here:
    https://dotnet.microsoft.com/en-us/download/dotnet/5.0
    """)
    input("Press any key to continue...\n")
    exit()

dd_release = requests.get("https://api.github.com/repos/SteamRE/DepotDownloader/releases/latest").json()["assets"][0]["browser_download_url"]
dd_release_name = dd_release.split("/")[-1]
urllib.request.urlretrieve(dd_release, os.getcwd() + "/" + dd_release_name)

with zipfile.ZipFile(os.getcwd() + "/" + dd_release_name, 'r') as zip_ref:
    zip_ref.extractall(os.getcwd() + "/depotdownloader")

os.remove(dd_release_name)

#don't look at me like that, i know this sounds very suspicious
print("\nIf the following does not work, you may need to disable steam guard.")

print("\nYour Steam username:")
username = input()
print("\nYour Steam password:")
password = input()

os.system("cd depotdownloader && dotnet DepotDownloader.dll -app 413150 -depot 413151 -manifest 7802000804251603756 -username {0} -password {1}".format(username, password))