# Hetzner VM Setup für Scratchy ASP.NET Core

## 🚀 Schnellstart

### 1. Auf der Hetzner VM (167.235.53.32) ausführen:

```bash
# Als root anmelden
ssh root@167.235.53.32

# Setup-Script herunterladen und ausführen
wget https://raw.githubusercontent.com/damiankaest/Scratchy/main/server-setup.sh
chmod +x server-setup.sh
./server-setup.sh
```

### 2. SSH Deploy Key einrichten:

```bash
# SSH-Schlüssel für GitHub Actions generieren
ssh-keygen -t rsa -b 4096 -f ~/.ssh/deploy_key

# Public Key zu authorized_keys hinzufügen
cat ~/.ssh/deploy_key.pub >> ~/.ssh/authorized_keys

# Private Key anzeigen (für GitHub Secret)
cat ~/.ssh/deploy_key
```

### 3. GitHub Secrets einrichten:

Gehe zu deinem GitHub Repository → Settings → Secrets and variables → Actions

**Benötigte Secrets:**

```bash
# SSH Deploy Key
DEPLOY_KEY = [Inhalt von ~/.ssh/deploy_key]

# Database & Services (aus deiner appsettings.json)
MONGODB_CONNECTION_STRING = "mongodb://adminuser:dk23Du08MTTJB@167.235.53.32:27017/admin"
MONGODB_DATABASE_NAME = "scratchUp"
BLOB_STORAGE_CONNECTION_STRING = "DefaultEndpointsProtocol=https;AccountName=scratchyuserimageacc;AccountKey=puskPpfuEsxhQJx+9RxgZbYtCvxza3wvUaKKOIbzKnb4/pfqDF1tU0R6IWDmdRVNJWNJkwEkyLWD+AStq1nm+Q==;EndpointSuffix=core.windows.net"
APPLICATIONINSIGHTS_CONNECTION_STRING = "InstrumentationKey=bd282cca-b01f-4ca3-971e-d2cb3c70a586;IngestionEndpoint=https://germanywestcentral-1.in.applicationinsights.azure.com/;LiveEndpoint=https://germanywestcentral.livediagnostics.monitor.azure.com/;ApplicationId=d2d8228c-cde9-42fb-8efa-ae48d9b418a6"

# JWT Secret
JWT_SECRET = "5d2f8a9fcd0e4b8b9e96c9a6f4e2c7d8"

# Firebase Service Account Key
FIREBASE_SERVICE_ACCOUNT_KEY = [Kompletter JSON-Inhalt der serviceAccountKey.json]
```

## 📦 Was wird auf der VM installiert:

- ✅ .NET 8.0 Runtime
- ✅ ASP.NET Core Runtime
- ✅ Nginx als Reverse Proxy
- ✅ Systemd Service für automatischen Start
- ✅ Firewall-Konfiguration
- ✅ Application Directory `/opt/scratchy`

## 🔧 Systemd Service

Die Anwendung läuft als `scratchy-app.service`:

```bash
# Service-Status prüfen
systemctl status scratchy-app

# Service starten/stoppen
systemctl start scratchy-app
systemctl stop scratchy-app
systemctl restart scratchy-app

# Logs anzeigen
journalctl -u scratchy-app -f
```

## 🌐 Nginx Konfiguration

- **URL**: `http://167.235.53.32`
- **Proxy**: Nginx → `localhost:5000`
- **Upload Limit**: 100MB
- **Konfiguration**: `/etc/nginx/sites-available/scratchy`

## 🔍 Debugging

### Anwendung läuft nicht:
```bash
# Logs prüfen
journalctl -u scratchy-app -f

# Permissions prüfen
ls -la /opt/scratchy/
chown -R www-data:www-data /opt/scratchy
chmod +x /opt/scratchy/Scratchy
```

### Nginx Probleme:
```bash
# Nginx-Status prüfen
systemctl status nginx

# Nginx-Konfiguration testen
nginx -t

# Nginx-Logs
tail -f /var/log/nginx/error.log
```

### Deployment-Probleme:
```bash
# GitHub Actions Deployment verfolgen
# Schaue in GitHub Actions Tab

# Manuelle Deployment-Simulation
cd /opt/scratchy
./Scratchy
```

## 🚀 Deployment-Prozess

1. **Push to main** → GitHub Actions startet
2. **Build & Test** → .NET Anwendung wird kompiliert
3. **Rsync** → Dateien werden auf VM synchronisiert
4. **Config** → Produktions-Konfiguration wird geschrieben
5. **Restart** → Service wird neu gestartet
6. **Verfügbar** → App läuft auf `http://167.235.53.32`

## 📊 Monitoring

```bash
# System-Status
htop
df -h
free -h

# Anwendungs-Logs
journalctl -u scratchy-app -f

# Nginx-Logs
tail -f /var/log/nginx/access.log
tail -f /var/log/nginx/error.log

# Netzwerk-Status
netstat -tlnp | grep :5000
netstat -tlnp | grep :80
```

## 🔒 Sicherheit

- ✅ Firewall aktiviert (Port 22, 80, 443)
- ✅ SSH Key Authentication
- ✅ Non-root User für Anwendung
- ⚠️ HTTP nur - für Produktion SSL einrichten

### SSL-Zertifikat einrichten (optional):
```bash
# Let's Encrypt installieren
apt install certbot python3-certbot-nginx

# SSL-Zertifikat für Domain
certbot --nginx -d your-domain.com

# Automatische Erneuerung
crontab -e
# Hinzufügen: 0 2 * * * certbot renew --quiet
```

## 🎯 Nach dem ersten Deployment

1. **Teste die API**: `curl http://167.235.53.32/api/health`
2. **Swagger UI**: `http://167.235.53.32/swagger`
3. **Logs prüfen**: `journalctl -u scratchy-app -f`
4. **Performance überwachen**: `htop`

**Deine Anwendung ist jetzt live auf: http://167.235.53.32** 🎉
