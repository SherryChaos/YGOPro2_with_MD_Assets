#!/bin/bash
set -x
set -o errexit

apt update && apt -y install wget git libarchive-tools

mkdir -p cdb expansions icon picture/card replay

# ygopro-database
git clone --depth=1 https://code.mycard.moe/mycard/ygopro-database
mv ./ygopro-database/locales/$TARGET_LOCALE/cards.cdb cdb/
cp -rf ./ygopro-database/locales/$TARGET_LOCALE/strings.conf config/

# lflist and translation
wget -O config/lflist.conf 'https://code.mycard.moe/mycard/ygopro/-/raw/master/lflist.conf?inline=false'
cp -rf locales/$TARGET_LOCALE/config/translation.conf ./config/

# script
git clone --depth=1 https://github.com/Fluorohydride/ygopro-scripts script

# ygopro-images
wget -O - https://cdn02.moecube.com:444/images/ygopro-images-${TARGET_LOCALE}.zip | bsdtar -C picture/card -xf -
mv picture/card/field picture/

# closeup
git clone --depth=1 https://code.mycard.moe/mycard/ygopro2-closeup
mv ygopro2-closeup/closeup picture/

# starter pack
git clone --depth=1 https://code.mycard.moe/mycard/ygopro-starter-pack
mv ygopro-starter-pack/deck ./deck
mv ygopro-starter-pack/single ./puzzle

mkdir pack
wget -O pack/pack.db https://cdn02.moecube.com:444/ygopro-card-list/pack.db
