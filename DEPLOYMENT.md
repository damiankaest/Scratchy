# Scratchy Deployment Guide

## Server Setup (Hetzner VM)

### 1. Vorbereitung der VM
```bash
# Script ausführbar machen und ausführen
chmod +x server-setup.sh
./server-setup.sh
```

### 2. Domain konfigurieren
```bash
sudo nano /etc/nginx/sites-available/scratchy
# Ersetze 'your-domain.com' mit deiner tatsächlichen Domain
```

### 3. SSL-Zertifikat einrichten (empfohlen)
```bash
sudo apt install certbot python3-certbot-nginx
sudo certbot --nginx -d your-domain.com
```

### 4. Firewall konfigurieren
```bash
sudo ufw allow 22/tcp
sudo ufw allow 80/tcp
sudo ufw allow 443/tcp
sudo ufw enable
```

## GitHub Secrets

Die folgenden Secrets müssen in deinem GitHub Repository unter Settings > Secrets and variables > Actions angelegt werden:

### Server-Verbindung
- `HOST`: IP-Adresse deiner Hetzner VM
- `USERNAME`: SSH-Benutzername (normalerweise 'root' oder dein Benutzername)
- `SSH_PRIVATE_KEY`: Dein privater SSH-Schlüssel (gesamter Inhalt der ~/.ssh/id_rsa Datei)
- `PORT`: SSH-Port (normalerweise 22)

### Datenbank & Services
- `MONGODB_CONNECTION_STRING`: MongoDB-Verbindungsstring
- `MONGODB_DATABASE_NAME`: Name deiner MongoDB-Datenbank
- `BLOB_STORAGE_CONNECTION_STRING`: Azure Blob Storage Verbindungsstring
- `APPLICATIONINSIGHTS_CONNECTION_STRING`: Application Insights Verbindungsstring

### Spotify API
- `SPOTIFY_CLIENT_ID`: Spotify API Client ID
- `SPOTIFY_CLIENT_SECRET`: Spotify API Client Secret

### Firebase
- `FIREBASE_SERVICE_ACCOUNT_KEY`: Kompletter Inhalt der serviceAccountKey.json Datei

## SSH-Schlüssel generieren

Falls du noch keinen SSH-Schlüssel hast:

```bash
# Auf deinem lokalen Computer
ssh-keygen -t rsa -b 4096 -c "your-email@example.com"

# Öffentlichen Schlüssel auf den Server kopieren
ssh-copy-id username@your-server-ip

# Privaten Schlüssel als GitHub Secret hinzufügen
cat ~/.ssh/id_rsa
```

## Deployment-Prozess

1. **Automatisches Deployment**: Push auf `main` oder `master` Branch triggert automatisches Deployment
2. **Build**: Anwendung wird kompiliert und getestet
3. **Deployment**: Anwendung wird auf den Server übertragen und gestartet
4. **Service**: Anwendung läuft als systemd-Service und startet automatisch bei Serverneustarts

## Monitoring

```bash
# Service-Status prüfen
sudo systemctl status scratchy-app

# Logs anzeigen
sudo journalctl -u scratchy-app -f

# Nginx-Status prüfen
sudo systemctl status nginx

# Nginx-Logs anzeigen
sudo tail -f /var/log/nginx/access.log
sudo tail -f /var/log/nginx/error.log
```

## Troubleshooting

### Service startet nicht
```bash
# Permissions prüfen
sudo chown -R www-data:www-data /var/www/scratchy
sudo chmod +x /var/www/scratchy/Scratchy

# Service neu starten
sudo systemctl restart scratchy-app
```

### Nginx Fehler
```bash
# Nginx-Konfiguration testen
sudo nginx -t

# Nginx neu laden
sudo systemctl reload nginx
```

### MongoDB-Verbindung
- Stelle sicher, dass MongoDB erreichbar ist
- Prüfe die Verbindungsstring-Syntax
- Überprüfe Firewall-Regeln

## Sicherheitshinweise

1. Verwende immer HTTPS in der Produktion
2. Halte das System aktuell: `sudo apt update && sudo apt upgrade`
3. Verwende starke Passwörter und SSH-Schlüssel
4. Aktiviere die Firewall
5. Überwache die Logs regelmäßig
