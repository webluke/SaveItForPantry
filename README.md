# Save It For Pantry

## !!! WIP Description !!!

```
This is a work in progress "Vibe Coded" description and setup instructions. Believe nothing it says for.
```

### Overview

Save It For Pantry is a modern web application built with .NET 9 and Blazor (Server-side rendering for interactive UI components), designed to streamline home pantry management. Users can track inventory items, generate dynamic shopping lists based on low-stock alerts or recipes, and effortlessly look up product details (such as nutritional info, prices, or alternatives) by scanning or entering UPC barcodes. The app integrates a robust backend powered by Entity Framework Core for efficient data handling and MySQL as the persistent database, ensuring scalability for household or small business use. Additional creative features include customizable categories (e.g., spices, canned goods), expiry date reminders via email notifications, and a visual dashboard for quick overviews with charts (leveraging Blazor's charting libraries).

Key features:
- **Pantry Management**: Add, edit, delete items with photos, quantities, and locations; automated stock level tracking.
- **Shopping Lists**: Use and Add to Shopping List, Track In Cart, Print Shopping List.
- **UPC Lookup**: API integration for real-time barcode scanning (mobile-friendly); supports fallback manual entry.
- **User Accounts WIP**: Secure authentication with role-based access (e.g., family sharing). 
- **Responsive Design**: Blazor's component-based architecture ensures seamless performance on desktops and mobiles.

The project is open-source and hosted on GitHub: [https://github.com/webluke/SaveItForPantry](https://github.com/webluke/SaveItForPantry).

### Setup Instructions (Ubuntu Linux with NGINX)

These steps assume a fresh Ubuntu 22.04 LTS (or later) server. You'll need root or sudo access. The app runs as a .NET web service, with MySQL for data storage and NGINX as a reverse proxy for HTTPS and load balancing. Total setup time: ~30-45 minutes.

#### 1. Prerequisites and System Updates
Update your system and install essential tools:
```
sudo apt update && sudo apt upgrade -y
sudo apt install -y curl wget unzip git
```

#### 2. Install .NET 9 SDK
Microsoft provides official .NET 9 support for Linux. Download and install:
```
wget https://dotnetsdk.azureedge.net/9.0.100-rc.2.24456.1/DotNet.SDK.9.0.100-rc.2.24456.1-linux-x64.tar.gz
sudo mkdir -p /usr/share/dotnet
sudo tar zxf DotNet.SDK.9.0.100-rc.2.24456.1-linux-x64.tar.gz -C /usr/share/dotnet
sudo ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
rm DotNet.SDK.9.0.100-rc.2.24456.1-linux-x64.tar.gz
```
Verify: `dotnet --version` (should show 9.0.x).

Install the ASP.NET Core runtime for production:
```
wget https://dotnet.microsoft.com/download/dotnet/9.0/runtime?installers=true
# Follow the tar.gz link for ASP.NET Core Runtime 9.0, download, and extract similarly to SDK
```

#### 3. Install MySQL Server
Install MySQL 8.0 (compatible with Entity Framework):
```
sudo apt install -y mysql-server
sudo systemctl start mysql
sudo systemctl enable mysql
sudo mysql_secure_installation  # Set root password and secure the install
```
Create the database for the app:
```
sudo mysql -u root -p
CREATE DATABASE SaveItForPantry;
CREATE USER 'pantryuser'@'localhost' IDENTIFIED BY 'your_secure_password';
GRANT ALL PRIVILEGES ON SaveItForPantry.* TO 'pantryuser'@'localhost';
FLUSH PRIVILEGES;
EXIT;
```

#### 4. Install and Configure NGINX
NGINX serves as the web server and reverse proxy:
```
sudo apt install -y nginx
sudo systemctl start nginx
sudo systemctl enable nginx
```
Configure NGINX for the Blazor app (edit `/etc/nginx/sites-available/default`):
```
sudo nano /etc/nginx/sites-available/default
```
Replace contents with (adjust `yourdomain.com` and ports as needed):
```
server {
    listen 80;
    server_name yourdomain.com;
    location / {
        proxy_pass http://localhost:5000;  # Blazor app port
        proxy_http_version 1.1;
        proxy_set_header Upgrade $http_upgrade;
        proxy_set_header Connection keep-alive;
        proxy_set_header Host $host;
        proxy_cache_bypass $http_upgrade;
        proxy_set_header X-Forwarded-For $proxy_add_x_forwarded_for;
        proxy_set_header X-Forwarded-Proto $scheme;
    }
}
```
Test and reload: `sudo nginx -t && sudo systemctl reload nginx`.

For HTTPS, install Certbot: `sudo apt install -y certbot python3-certbot-nginx`, then `sudo certbot --nginx`.

#### 5. Clone and Build the Project
```
git clone https://github.com/webluke/SaveItForPantry.git
cd SaveItForPantry
```
Restore dependencies and build (Entity Framework will handle migrations):
```
dotnet restore
dotnet ef database update  # Ensure MySQL connection string is set in appsettings.json (e.g., "Server=localhost;Database=SaveItForPantry;User=pantryuser;Password=your_secure_password;")
dotnet build --configuration Release
```

Update `appsettings.json` for production:
- Set `ConnectionStrings:DefaultConnection` to match your MySQL setup.
- Configure `Kestrel` for the port (e.g., 5000).
- Add logging and any app-specific secrets (use User Secrets in dev, environment vars in prod).

#### 6. Run the Application
For development/testing:
```
dotnet run --project SaveItForPantry.Web
```
Access at `http://localhost:5000` (or your server IP).

For production (as a systemd service for auto-start):
Create `/etc/systemd/system/pantryapp.service`:
```
[Unit]
Description=Save It For Pantry App
After=network.target

[Service]
Type=notify
ExecStart=/usr/bin/dotnet /path/to/SaveItForPantry/SaveItForPantry.Web/bin/Release/net9.0/SaveItForPantry.Web.dll
Restart=always
RestartSec=10
Environment=ASPNETCORE_ENVIRONMENT=Production

[Install]
WantedBy=multi-user.target
```
Replace `/path/to/` with your actual path. Then:
```
sudo systemctl daemon-reload
sudo systemctl start pantryapp
sudo systemctl enable pantryapp
```
Monitor logs: `sudo journalctl -u pantryapp -f`.

#### 7. Final Checks and Troubleshooting
- Visit `http://yourdomain.com` (or server IP) in a browser—NGINX should proxy to the Blazor app.
- Common issues:
  - MySQL connection: Verify firewall (`sudo ufw allow mysql`) and connection string.
  - Permissions: `sudo chown -R www-data:www-data /path/to/SaveItForPantry` for web files.
  - .NET errors: Check `dotnet --info` for compatibility.
- For scaling, consider Dockerizing (add a `Dockerfile` for .NET 9 + MySQL images) or PM2-like tools, but systemd suffices for Ubuntu.

If you encounter errors, share the output—happy to debug! This setup keeps things lightweight and performant on Linux.