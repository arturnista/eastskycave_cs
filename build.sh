echo "STARTING BUILD"

rm -rf ./bin/Release/net6.0/publish/
dotnet publish --configuration Release

echo "Copying images..."
mkdir -p ./bin/Release/net6.0/publish/resources/images/
cp resources/images/*.png ./bin/Release/net6.0/publish/resources/images/

echo "Copying levels..."
mkdir -p ./bin/Release/net6.0/publish/resources/map/levels/
cp resources/map/levels/*.json ./bin/Release/net6.0/publish/resources/map/levels/

echo "Copying settings..."
mkdir -p ./bin/Release/net6.0/publish/resources/settings/
cp resources/settings/*.json ./bin/Release/net6.0/publish/resources/settings/

rm -rf ./bin/Release/net6.0/publish/EastSkyMountain.pdb

rm -rf ./bin/Release/windows

mv ./bin/Release/net6.0/publish/ ./bin/Release/windows/

echo "Finished building"
