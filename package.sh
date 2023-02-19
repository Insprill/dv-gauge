INFO_FILE="info.json"
GAUGE_DLL="bin/Release/netframework4.8/Gauge.dll"
DISPLAY_NAME=$(jq -r '.DisplayName' $INFO_FILE)
VERSION=$(jq -r '.Version' $INFO_FILE)

if ! [ -e $GAUGE_DLL ]; then
    echo "Failed to find $GAUGE_DLL! Have you built it in release mode yet?"
    exit 1
fi

zip -1 -T -j -u "${DISPLAY_NAME}_$VERSION.zip" $GAUGE_DLL info.json
