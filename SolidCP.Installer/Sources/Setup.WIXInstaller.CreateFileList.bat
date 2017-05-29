
"%WIX%\bin\heat.exe" dir ..\..\SolidCP\Build\release\EnterpriseServer -o Setup.WIXInstaller\EnterpriseServerFiles.wxs -gg -sreg -srd -var wix.BUILDESPATH -cg EnterpriseServerFiles -dr INSTALLENTERPRISESERVERFOLDER

"%WIX%\bin\heat.exe" dir ..\..\SolidCP\Build\release\Server -o Setup.WIXInstaller\ServerFiles.wxs -gg -sreg -srd -var wix.BUILDSPATH -cg ServerFiles -dr INSTALLSERVERFOLDER

"%WIX%\bin\heat.exe" dir ..\..\SolidCP\Build\release\Server.asp2.0 -o Setup.WIXInstaller\ServerFilesaspv2.wxs -gg -sreg -srd -var wix.BUILDSPATH -cg Serveraspv2Files -dr INSTALLSERVERASPV2FOLDER

"%WIX%\bin\heat.exe" dir ..\..\SolidCP\Build\release\Portal -o Setup.WIXInstaller\PortalFiles.wxs -gg -sreg -srd -var wix.BUILDPPATH -cg PortalFiles -dr INSTALLPORTALFOLDER

"%WIX%\bin\heat.exe" dir ..\..\SolidCP\Build\release\WebDavPortal -o Setup.WIXInstaller\WebDavPortalFiles.wxs -gg -sreg -srd -var wix.BUILDWDPPATH -cg WebDavPortalFiles -dr INSTALLWEBDAVPORTALFOLDER
