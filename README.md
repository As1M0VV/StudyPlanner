# Study Planner

ASP.NET Core MVC ile hazirlanmis ders calisma ve sinav takip sistemi.

## Kullanilan Teknolojiler

- HTML
- CSS
- JavaScript
- Bootstrap
- ASP.NET Core MVC
- SQLite
- Entity Framework Core

## Ana Sayfalar

- Anasayfa: dashboard, yaklasan calismalar ve sinavlar
- Dersler: ders ekleme, duzenleme ve silme
- Calisma Plani: derslere bagli calisma gorevleri
- Sinavlar: sinav tarihi, konu ve not takibi
- Istatistikler: toplam calisma suresi ve ders bazli dagilim

## Calistirma

Projeyi ilk calistirdiginda `studyplanner.db` dosyasi otomatik olusturulur ve ornek veriler eklenir.

```bash
dotnet restore
dotnet run --framework net10.0 --urls http://localhost:5111
```

Lab bilgisayarinda .NET 8 kuruluysa su komut da kullanilabilir:

```bash
dotnet run --framework net8.0 --urls http://localhost:5111
```

Tarayicida ac:

```text
http://localhost:5111
```

## Render ile Yayina Alma

Bu proje Dockerfile ile Render uzerinde calistirilabilir.

1. Projeyi GitHub reposuna yukle.
2. Render Dashboard uzerinden `New` > `Web Service` sec.
3. GitHub reposunu bagla.
4. Runtime/Language alaninda `Docker` sec.
5. Dockerfile path olarak proje kokundeki `Dockerfile` kullan.
6. Deploy baslat.

Render web servislerinde HTTP sunucusu `PORT` ortam degiskenine baglanmalidir. Bu proje Dockerfile icinde bunu otomatik yapar:

```bash
dotnet StudyPlanner.dll --urls "http://0.0.0.0:${PORT:-10000}"
```

Render deploy tamamlandiginda sana `https://...onrender.com` seklinde bir adres verir.
