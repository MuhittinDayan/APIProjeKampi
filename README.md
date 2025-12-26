# 🍽️ APIProjeKampi: Yummy & Otika

> **Yummy: Uçtan Uca Restoran Platformu**

Yummy ve Otika gibi modern arayüz temaları ve tekrar kullanılabilir Razor bileşenleri ile güçlendirilen uygulama; hem şık bir tanıtım yüzü hem de güçlü bir arka plan sunarak gerçek hayat senaryolarına uygun, tam donanımlı bir **full-stack** çözüm sağlar.

Bu proje, **ASP.NET Core 6.0 Web API** mimarisi üzerinde, **Google Gemini** ve **Hugging Face** yapay zeka teknolojilerini birleştiren modern bir restoran yönetim sistemidir.

---

## 🤖 Yapay Zeka Entegrasyonları (AI Powered)

Projenin en güçlü yanı, yapay zeka destekli akıllı özellikleridir:

* **💬 Akıllı Asistan:** Google Gemini **(2.5 Flash)** modeli ile entegre, restoranla ilgili soruları (menü, saatler vb.) yanıtlayan canlı sohbet botu (SignalR üzerinden).
* **🥗 AI Yemek Önerisi:** Google Gemini **(2.0 Flash)** kullanılarak, kullanıcının elindeki malzemelere göre yaratıcı tarifler sunan akıllı modül.
* **🛡️ İçerik Moderasyonu:** Hugging Face modelleri ile gelen mesajlarda ve yorumlarda toksik içerik/uygunsuzluk denetimi.

---

## 🚀 Temel Fonksiyonlar

Uygulama, bir restoranın dijital ihtiyaçlarını karşılayan kapsamlı modüller içerir:

* **Yönetim:** Ürün, kategori, şef profilleri, galeri ve referans işlemleri.
* **Etkileşim:** Online masa rezervasyonu, etkinlik yönetimi ve Google Maps entegrasyonu.
* **İletişim:** Gerçek zamanlı bildirimler, mesaj kutusu ve personel görev takibi.

---

## ⚙️ Teknik Altyapı ve Mimari

Sağlam bir backend mimarisi üzerine inşa edilmiştir:

* **Core Mimari:** ASP.NET Core 6.0, Code First yaklaşımı ve Entity Framework Core.
* **Veritabanı & Güvenlik:** MSSQL Server, Migration yapısı ve Identity kütüphanesi.
* **Arayüz & Dokümantasyon:** Responsive Web UI, Admin Dashboard (Grafik & İstatistikler) ve Swagger API dokümantasyonu.

---

## 🛠️ Kullanılan Teknolojiler

### 💻 Backend & Core

* .NET 6.0, ASP.NET Core Web API
* Entity Framework Core (Code First) & MSSQL Server
* SignalR (Gerçek zamanlı iletişim)
* Swagger, AutoMapper, FluentValidation

### 🎨 Frontend & UI

* Razor Pages, Bootstrap 5
* jQuery, AJAX
* ApexCharts & amCharts (Grafik & İstatistikler)
* Google Maps API

### 🧠 Yapay Zeka (AI) Modelleri

* **Google Gemini (2.5 Flash):** Akıllı sohbet asistanı (ChatHub).
* **Google Gemini (2.0 Flash):** Malzemeye göre tarif üretimi.
* **Hugging Face:** Mesajlarda toksik içerik moderasyonu.

---

## 📂 Proje Dizin Yapısı

Proje, Backend ve Frontend katmanlarının net bir şekilde ayrıldığı modüler bir yapıya sahiptir:

```plaintext
📦 ApiProjeKampi
├── 🔧 ApiProjeKampi_WebApi     # Backend: RESTful API & Veri Yönetimi
│   ├── Controllers/            # API Uç Noktaları (Endpoints)
│   ├── Context/                # Veritabanı Bağlamı (EF Core)
│   ├── Entities/               # Veritabanı Tablo Karşılıkları
│   ├── DTOs/                   # Veri Transfer Objeleri
│   ├── Mapping/                # AutoMapper Yapılandırmaları
│   ├── Migrations/             # Veritabanı Sürüm Geçmişi
│   └── ValidationRules/        # Doğrulama Kuralları (FluentValidation)
│
└── 🌐 ApiProjeKampi.WebUI      # Frontend: MVC Arayüz & Kullanıcı Etkileşimi
    ├── Controllers/            # Sayfa Yönlendirmeleri ve API Tüketimi
    ├── Models/                 # ViewModel'ler ve SignalR Hub (Chat)
    ├── ViewComponents/         # Tekrar Kullanılabilir Arayüz Parçaları
    ├── Views/                  # Razor Sayfaları (HTML)
    ├── DTOs/                   # Arayüz Odaklı Veri Objeleri
    └── wwwroot/                # Statik Dosyalar (Yummy/Otika Temaları, CSS, JS)

```

---

## 📸 Proje Ekran Görüntüleri (Screenshots)
<details> <summary><b>🖥️ Kullanıcı Arayüzü (Web UI) - Tıkla ve Gör</b></summary>


Müşterilerin karşılandığı modern arayüz sayfaları (Anasayfa, Menü, Hakkımızda, İletişim, Galeri vb.)

<img src="https://github.com/user-attachments/assets/39e9b55b-64aa-422e-8a24-2ac78628fca0" width="100%" alt="Anasayfa">
<img src="https://github.com/user-attachments/assets/5ad74b82-2f94-4ca1-a64a-f345cc8578c6" width="48%" alt="Hakkımızda">
<img src="https://github.com/user-attachments/assets/9c99059a-74f0-49e2-9fe5-05a614995e5f" width="48%" alt="Hakkımızda-2">
<img src="https://github.com/user-attachments/assets/30f7d562-00dd-46f3-ab1c-ee8e4e8b6d1d" width="48%" alt="Menü">
<img src="https://github.com/user-attachments/assets/77461cff-ac15-4f69-bbc6-42c35e202db7" width="48%" alt="Müşteri Yorumları"> 
<img src="https://github.com/user-attachments/assets/28189c02-6a0e-4385-8fd0-212c062b0afe" width="48%" alt="Etkinlikler"> 
<img src="https://github.com/user-attachments/assets/c28bcb21-a91f-4fad-883c-2862125be1f2" width="48%" alt="Şefler">
<img src="https://github.com/user-attachments/assets/7c8e5706-35dc-4513-afa7-cd3f66d411b2" width="48%" alt="Rezervasyon">
<img src="https://github.com/user-attachments/assets/3e06e185-8e20-472e-883c-51b429e24eed" width="48%" alt="Galeri"> 
<img src="https://github.com/user-attachments/assets/c137d145-2e1f-4733-911a-e6e7b94a945a" width="48%" alt="Google Maps"> 
<img src="https://github.com/user-attachments/assets/76a57aab-c0a7-4a94-85b9-f268146a2baa" width="48%" alt="İletişim"> </details>

<details> <summary><b>📊 Admin Dashboard (Yönetim Paneli) - Tıkla ve Gör</b></summary>


Veri girişi, istatistikler, bildirimler ve yönetim araçları.

<img src="https://github.com/user-attachments/assets/8f6811da-0ef1-4d70-b567-9ba46c0e1597" width="100%" alt="Dashboard Full">
<img src="https://github.com/user-attachments/assets/67d8137a-20f4-47fc-b3d0-61e3af56f13a" width="48%" alt="Kategoriler"> 
<img src="https://github.com/user-attachments/assets/0e0ff5af-5f8c-4c0b-a470-6ad0d1f2381f" width="48%" alt="Ürünler"> 
<img src="https://github.com/user-attachments/assets/00adbb31-188c-4c0c-9f85-1c42adeedbbb" width="48%" alt="Feature"> 
<img src="https://github.com/user-attachments/assets/dfb03b3f-03c0-4d42-bf2d-fca239f335b2" width="48%" alt="Hakkımda"> 
<img src="https://github.com/user-attachments/assets/81919666-6c00-4ea5-be4c-45544625c557" width="48%" alt="Why Choose Yummy"> 
<img src="https://github.com/user-attachments/assets/fe3c2b53-654e-44a3-ab5b-89887c12ff52" width="48%" alt="Mesajlar"> 
<img src="https://github.com/user-attachments/assets/d9bb12e4-f6a9-46fc-adc8-2222f6ccd2c8" width="48%" alt="Etkinlikler"> 
<img src="https://github.com/user-attachments/assets/d610deb2-c993-44fc-b5b7-1574ec8f1bee" width="48%" alt="Şefler"> 
<img src="https://github.com/user-attachments/assets/8ebf89c6-4853-4394-946c-f6db9ee4c038" width="48%" alt="Rezervasyonlar"> 
<img src="https://github.com/user-attachments/assets/530c60a4-e4a4-4c3f-bdce-6b4aa162a9e0" width="48%" alt="Galeri"> 
<img src="https://github.com/user-attachments/assets/9101e156-5c98-4de7-a4fb-49964e1061e8" width="48%" alt="İletişim"> 
<img src="https://github.com/user-attachments/assets/8bbd1de9-d505-45ff-ae2c-0c736c9613d7" width="48%" alt="Gelen Mesajlar"> 
<img src="https://github.com/user-attachments/assets/a22ca6c3-0af8-4320-b24c-37a64c4c3fa7" width="48%" alt="Otomatik Yanıt Sayfası"> </details>

<details> <summary><b>🤖 Yapay Zeka Özellikleri (AI Features) - Tıkla ve Gör</b></summary>

Google Gemini ile malzeme analizi ve tarif oluşturma ekranı.

<img src="https://github.com/user-attachments/assets/e9991982-32b6-49ff-8831-7cc4f825584d" width="100%" alt="AI Recipe Generator">

</details>

## ⚙️ Kurulum ve Çalıştırma (Installation)

Projeyi yerel ortamınızda çalıştırmak için aşağıdaki adımları izleyin.

### 1. Gereksinimler

* [.NET 6.0 SDK](https://dotnet.microsoft.com/download/dotnet/6.0)
* [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (LocalDB veya Express)
* Visual Studio 2022 (Önerilen)

### 2. Projeyi Klonlayın

```bash
git clone https://github.com/MuhittinDayan/ApiProjeKampi.git
cd ApiProjeKampi

```

### 3. Yapılandırma (appsettings.json)

`ApiProjeKampi_WebApi` ve `ApiProjeKampi.WebUI` projeleri içerisindeki `appsettings.json` dosyalarını kendi ortamınıza göre düzenleyin.

**Özellikle API Katmanında (`ApiProjeKampi_WebApi`):**

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER_NAME;Database=ApiYummyDb;Trusted_Connection=True;"
  },
  "Gemini": {
    "ApiKey": "BURAYA_GOOGLE_AI_STUDIO_API_KEY_GELECEK"
  },
  "HuggingFace": {
    "ApiKey": "BURAYA_HUGGING_FACE_API_KEY_GELECEK"
  }
}

```

### 4. Veritabanını Oluşturma

Proje **Code First** yaklaşımı kullandığı için veritabanını migration'lar ile oluşturmalısınız.
Visual Studio'da **Package Manager Console** (PMC) üzerinden şu komutu çalıştırın:

> **Dikkat:** `Default Project` olarak **ApiProjeKampi_WebApi** seçili olduğundan emin olun.

```powershell
Update-Database

```

### 5. Uygulamayı Ayağa Kaldırma

Bu çözüm **Web API** ve **Web UI** olmak üzere iki ayrı projeden oluşur. Uygulamanın düzgün çalışması için ikisinin de aynı anda çalışması gerekir.

1. Solution Explorer'da **Solution 'ApiProjeKampi'** üzerine sağ tıklayın.
2. **Properties (Özellikler) > Startup Project (Başlangıç Projesi)** menüsüne gidin.
3. **Multiple startup projects (Çoklu başlangıç projesi)** seçeneğini işaretleyin.
4. Her iki proje için de Action kısmını **Start** olarak ayarlayın.
5. Projeyi çalıştırın (F5).

---
