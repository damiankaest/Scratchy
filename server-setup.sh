#!/bin/bash

# Scratchy ASP.NET Core Application Server Setup Script for Hetzner VM
# Run this script on your Hetzner VM (167.235.53.32) to prepare it for deployments

set -e

echo "Setting up Scratchy ASP.NET Core Application Server..."

# Update system
apt update && apt upgrade -y

# Install .NET 8.0 Runtime
wget https://packages.microsoft.com/config/ubuntu/22.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
dpkg -i packages-microsoft-prod.deb
rm packages-microsoft-prod.deb

apt update
apt install -y aspnetcore-runtime-8.0 dotnet-runtime-8.0

# Install nginx as reverse proxy
apt install -y nginx

# Create application directory
mkdir -p /opt/scratchy
chown -R www-data:www-data /opt/scratchy

# Create systemd service file
tee /etc/systemd/system/scratchy-app.service > /dev/null <<EOF
[Unit]
Description=Scratchy ASP.NET Core Application
After=network.target

[Service]
Type=notify
User=www-data
Group=www-data
WorkingDirectory=/opt/scratchy
ExecStart=/opt/scratchy/Scratchy
Restart=always
RestartSec=10
KillSignal=SIGINT
SyslogIdentifier=scratchy-app
Environment=ASPNETCORE_ENVIRONMENT=Production
Environment=ASPNETCORE_URLS=http://localhost:5000

[Install]
WantedBy=multi-user.target
EOF

# Configure nginx
tee /etc/nginx/sites-available/scratchy > /dev/null <<EOF
server {
    listen 80;
    server_name 167.235.53.32;  # Your VM IP

    location / {
        proxy_pass http://localhost:5000;
        proxy_http_version 1.1;
        proxy_set_header Upgrade \$http_upgrade;
        proxy_set_header Connection keep-alive;
        proxy_set_header Host \$host;
        proxy_cache_bypass \$http_upgrade;
        proxy_set_header X-Forwarded-For \$proxy_add_x_forwarded_for;
        proxy_set_header X-Forwarded-Proto \$scheme;
        proxy_set_header X-Real-IP \$remote_addr;
        
        # File upload size limit (100MB)
        client_max_body_size 100M;
        
        # Timeout settings
        proxy_connect_timeout 60s;
        proxy_send_timeout 60s;
        proxy_read_timeout 60s;
    }
}
EOF

# Enable nginx site
ln -sf /etc/nginx/sites-available/scratchy /etc/nginx/sites-enabled/
rm -f /etc/nginx/sites-enabled/default

# Test nginx configuration
nginx -t

# Configure firewall
ufw allow 22/tcp
ufw allow 80/tcp
ufw allow 443/tcp
ufw --force enable

# Reload systemd and start services
systemctl daemon-reload
systemctl restart nginx
systemctl enable nginx

echo "✅ Server setup complete!"
echo ""
echo "📋 Next steps:"
echo "1. Add your GitHub Deploy Key to the VM:"
echo "   - Generate SSH key: ssh-keygen -t rsa -b 4096 -f ~/.ssh/deploy_key"
echo "   - Add public key to authorized_keys: cat ~/.ssh/deploy_key.pub >> ~/.ssh/authorized_keys"
echo "   - Add private key content to GitHub Secret: DEPLOY_KEY"
echo ""
echo "2. Set up GitHub Secrets in your repository:"
echo "   - DEPLOY_KEY (private SSH key)"
echo "   - MONGODB_CONNECTION_STRING"
echo "   - MONGODB_DATABASE_NAME"
echo "   - BLOB_STORAGE_CONNECTION_STRING"
echo "   - APPLICATIONINSIGHTS_CONNECTION_STRING"
echo "   - JWT_SECRET"
echo "   - FIREBASE_SERVICE_ACCOUNT_KEY"
echo ""
echo "3. Test the setup:"
echo "   - Push to main branch"
echo "   - Check deployment at: http://167.235.53.32"
echo ""
echo "4. Optional: Set up domain and SSL certificate"
