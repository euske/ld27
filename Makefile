# Makefile

RM=rm -f
CP=cp -f
MV=mv -f
RSYNC=rsync -av

# Project settings
TARGET=ld27
URLBASE=ludumdare.tabesugi.net:public/file/ludumdare.tabesugi.net/$(TARGET)

all: 

update: $(TARGET)
	-$(MV) $(TARGET)/$(TARGET).html $(TARGET)/index.html
	$(RSYNC) $(TARGET)/ $(URLBASE)/
