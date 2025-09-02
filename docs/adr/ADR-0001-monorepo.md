# ADR-0001: Monorepo Adopted

- Status: Accepted
- Date: 2025-09-02

## Context
TURNA96 kapsamındaki tüm servisler ve altyapı bileşenleri için tek bir kod deposu yönetimi hedeflenmektedir. Dağınık repolar süreç karmaşıklığı ve entegrasyon zorluğu yaratmaktadır.

## Decision
Projede monorepo yaklaşımı benimsendi. Tüm servisler, altyapı kodları ve dokümantasyon aynı depoda tutulacaktır.

## Consequences
- Versiyon uyumu ve ortak bağımlılık yönetimi kolaylaşır.
- Tek noktadan CI/CD ve kod inceleme süreçleri uygulanabilir.
- Depo büyüdükçe otomasyon ve araç gereksinimi artabilir.
