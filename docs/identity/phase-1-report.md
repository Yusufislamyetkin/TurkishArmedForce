# Faz 1 – Kimlik ve Oturum Altyapısı

Bu fazda Turna96 Interview Studio için modern bir kimlik doğrulama katmanı inşa edildi. ASP.NET Core Identity kullanılarak adayların mülakat simülasyonlarına güvenle erişebilmesi sağlandı.

## Öne Çıkanlar

- **ASP.NET Core Identity + SQL Server:** EF Core tabanlı ApplicationDbContext ile güçlü bir kullanıcı yönetimi temeli kuruldu. Parola politikaları, kilitlenme eşikleri ve iki faktörlü doğrulama senaryoları yapılandırıldı.
- **Kişiselleştirilmiş aday profili:** Identity kullanıcısı genişletilerek adayların `FullName`, `CurrentRole` ve `TargetLevel` bilgileri tutulur hale getirildi. Profil yönetim ekranı bu alanları güncelleme desteği sunar.
- **Tema uyumlu arayüzler:** Giriş, kayıt, parola sıfırlama ve hesap yönetimi sayfaları mülakat eğitim platformuna uygun koyu temalı, motivasyon odaklı bir tasarımla hazırlandı. Bootstrap 5 ve özel CSS ile card tabanlı modern bir UI oluşturuldu.
- **Debug e-posta gönderimi:** Geliştirme aşamasında e-posta doğrulama ve parola sıfırlama akışlarını izlemek için `DebugEmailSender` servisi tanımlandı. Gerçek SMTP entegrasyonu için genişletilebilir yapı hazır.
- **Hazır 2FA adımları:** Authenticator kodu ve kurtarma kodu ile giriş ekranları aktifleştirildi; ilerleyen fazlarda MFA politikaları kolaylıkla devreye alınabilir.

## Kurulum ve Çalıştırma

1. SQL Server örneğinizi ayarlayın (Docker veya yerel).
2. `appsettings.json` içindeki `DefaultConnection` değerini güncel veritabanınıza göre değiştirin.
3. İlk şema için EF Core migration oluşturun:
   ```bash
   dotnet ef migrations add InitialIdentity --project services/turna96/InterviewPrep.Web
   dotnet ef database update --project services/turna96/InterviewPrep.Web
   ```
4. Uygulamayı çalıştırın:
   ```bash
   dotnet run --project services/turna96/InterviewPrep.Web
   ```

## Sonraki Adımlar

- OpenAI Assistants API ile kişiselleştirilmiş soru üretimi.
- Gelişmiş ilerleme panosu ve oyunlaştırma bileşenlerinin devreye alınması.
- Gerçek e-posta/SMS sağlayıcısı ile MFA entegrasyonu.
- Admin paneli ve soru yönetimi için ayrı modüllerin geliştirilmesi.
