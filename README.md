# TURNA96 Monorepo

TURNA96 kurumsal ölçekte, yüksek güvenlik ve performans hedefleyen bir mesajlaşma mikroservisidir. Gerçek zamanlı iletişim için SignalR ve Redis backplane, kalıcı veri saklama için PostgreSQL ve EF Core kullanmayı amaçlar. Tam metin arama (Elasticsearch), yapılandırılmış loglama (Serilog), güvenli kimlik doğrulama (OpenIddict), trafik yönlendirme (YARP) ve kapsamlı gözlemlenebilirlik (OpenTelemetry, Prometheus, Grafana) bileşenleri projenin temelini oluşturur.

## Klasör Yapısı
- `services/turna96` – ana mesajlaşma servisi (Faz 1'de doldurulacak)
- `services/gateway` – YARP tabanlı API gateway (Faz 7'de)
- `ops/compose` – Docker Compose geliştirme ortamı
- `docs/adr` – Architecture Decision Records
- `docs/arch` – mimari diyagramlar ve dokümantasyon
- `scripts` – yardımcı betikler

## Faz Planı
1. Monorepo iskeleti
2. Solution + SharedKernel
3. Domain katmanı
4. Application katmanı
5. Infrastructure katmanı
6. API katmanı
7. Realtime (SignalR + Redis)
8. Gateway (YARP)
9. Observability (OTel + Prometheus + Grafana)
10. Security hardening
11. Docker Compose + Kubernetes dağıtımı

Proje, fazlar ilerledikçe genişleyecek ve kurumsal düzeyde ölçeklenebilir, güvenli bir mesajlaşma altyapısı oluşturacaktır.
