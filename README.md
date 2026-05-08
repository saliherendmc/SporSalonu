# 🏋️ Spor Salonu Otomasyon Sistemi

ASP.NET Core MVC mimarisi kullanılarak geliştirilmiş, spor salonlarının üye kayıt, takip ve ödeme süreçlerini kolaylaştırmayı amaçlayan bir yönetim panelidir.

## 🌟 Temel Özellikler

*   **Üye Yönetimi:** Detaylı üye kaydı, bilgi güncelleme ve pasif üye silme işlemleri.
*   **Ölçüm Takibi:** Üyelerin gelişimlerini takip edebilmek için boy, kilo ve vücut kitle indeksi kayıtları.
*   **Program Atama:** Üyelere özel antrenman ve beslenme planlarının sisteme tanımlanması.
*   **Ödeme Sistemi:** Üye aidatlarının takibi, yeni ödeme girişi ve geçmiş ödeme dökümleri.
*   **Dinamik Tablolar:** Tüm listeleme sayfalarında gelişmiş arama ve filtreleme (DataTables).

## 💻 Kullanılan Teknolojiler

*   **Backend:** .NET 8.0 (ASP.NET Core MVC)
*   **ORM:** Entity Framework Core
*   **Veritabanı:** MS SQL Server
*   **Frontend:** Bootstrap 5, FontAwesome, JQuery
*   **UI Bileşenleri:** DataTables, CSS3 Kart Tasarımları

## 🛠️ Kurulum Notları

1. Projeyi bilgisayarınıza indirin.
2. `appsettings.json` dosyasındaki `ConnectionStrings` alanını kendi SQL Server adresinize göre düzenleyin.
3. Visual Studio - Package Manager Console üzerinden `Update-Database` komutunu çalıştırarak tabloları oluşturun.
4. Projeyi `F5` ile çalıştırın.

---
**Geliştirici:** [Salih Eren DAMACI](https://github.com/saliherendmc)
