#!/bin/bash

function check_file_exists() {
    if ! [ -e "$1" ]; then
        echo "Failed to find $1!"
        exit 1
    fi
}

INFO_FILE="info.json"
GAUGE_DLL="build/Gauge.dll"
DISPLAY_NAME=$(jq -r '.DisplayName' $INFO_FILE)
VERSION=$(jq -r '.Version' $INFO_FILE)

check_file_exists "$GAUGE_DLL"
check_file_exists "$INFO_FILE"

zip -1 -T -j -u "${DISPLAY_NAME}_v$VERSION.zip" $GAUGE_DLL $INFO_FILE
