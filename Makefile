# Makefile

RM=rm -f
CP=cp -f
MV=mv -f
TAR=tar
RSYNC=rsync -av

# Project settings
TARGET=ld27
URLBASE=ludumdare.tabesugi.net:public/file/ludumdare.tabesugi.net/$(TARGET)

all: 

update: $(TARGET)
	-$(MV) $(TARGET)/$(TARGET).html $(TARGET)/index.html
	$(RSYNC) $(TARGET)/ $(URLBASE)/

ld27_euske_linux.tar.gz: ld27_euske_linux
	$(TAR) zcf ld27_euske_linux.tar.gz ld27_euske_linux
update_linux: ld27_euske_linux.tar.gz
	$(RSYNC) ld27_euske_linux.tar.gz $(URLBASE)/
clean:
	-$(RM) ld27_euske_linux.tar.gz
