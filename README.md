E-Learning API — واجهة برمجية لإدارة التسجيل في الكورسات
واجهة RESTful API مبنية بـ ASP.NET Core 10 لإدارة تسجيل الموظفين في الكورسات مع نظام موافقة، مصممة لمنصات التعلم الإلكتروني الحكومية.
---
⚡ تشغيل المشروع بشكل فوري
> **قاعدة البيانات جاهزة ومرفوعة مسبقاً على السيرفر — لا داعي لأي إعداد إضافي.**
المتطلبات الوحيدة
خطوات التشغيل
```bash
# 1. استنسخ المشروع
git clone <your-repo-url>
cd E-Learning

# 2. شغّل مباشرة
dotnet run
```
فتح Swagger UI
بعد التشغيل افتح المتصفح على:
```
```
---
🗄️ قاعدة البيانات
قاعدة البيانات مرفوعة مسبقاً على سيرفر SQL Server وجاهزة للاستخدام الفوري.
```
```
الـ Connection String موجود بالفعل في `appsettings.json` ولا يحتاج أي تعديل:
```
🏗️ هيكل المشروع والقرارات المعمارية
هيكل الملفات
```
E-Learning.API/
├── Controllers/
│   ├── CoursesController.cs
│   ├── LearnersController.cs
│   └── EnrollmentsController.cs
├── Services/           # منطق الأعمال (Business Logic)
│   ├── CourseService.cs
│   ├── LearnerService.cs
│   └── EnrollmentService.cs
├── Models/             # كيانات EF Core
│   ├── Course.cs
│   ├── Learner.cs
│   ├── Enrollment.cs
│   ├── AuditLog.cs
│   └── UserContext.cs
├── DTOs/               
│   ├── CourseDtos.cs
│   ├── LearnerDtos.cs
│   └── EnrollmentDtos.cs 
├── Data/
│   └── AppDbContext.cs
├── Middleware/
│   ├── GlobalExceptionMiddleware.cs  # يمسك كل الأخطاء غير المتوقعة
│   └── UserContextMiddleware.cs      # يقرأ X-User-Role / X-User-Id من الـ Headers
└── Migrations/
```
أبرز القرارات المعمارية
Service Layer Pattern: الـ Controllers لا تحتوي على أي business logic — كل القواعد موجودة في الـ Services.
ServiceResult<T>: Wrapper موحّد لنتائج الـ Services بدل throw exceptions للأخطاء المتوقعة.
Role Simulation عبر Headers: الدور يُمرَّر في `X-User-Role` و `X-User-Id`، ويُحوَّل إلى `UserContext` بواسطة Middleware.
Enum كـ String في الـ DB: `EnrollmentStatus` مخزون كنص لسهولة القراءة في قاعدة البيانات.
Audit Log: كل قرار (Approve/Reject) يُسجَّل تلقائياً مع القيم القديمة والجديدة بصيغة JSON.
Pagination: كل الـ List endpoints تدعم `?page=1&pageSize=10` مع إجمالي العدد.
---
🔐 محاكاة الصلاحيات (Role Simulation)
كل الطلبات يجب أن تحتوي على هذين الـ Headers:
Header	القيم المقبولة
`X-User-Role`	`Admin` أو `Manager` أو `Learner`
`X-User-Id`	أي رقم صحيح (مثل `1`، `5`)
جدول الصلاحيات
العملية	Admin	Manager	Learner
إنشاء / تعديل / حذف كورس	✅	❌	❌
عرض الكورسات والمتعلمين	✅	✅	✅
إضافة متعلم	✅	✅	✅
التسجيل في كورس	❌	❌	✅
الموافقة أو الرفض	❌	✅	❌
---
📡 الـ API Endpoints
Courses — الكورسات
Method	Endpoint	الوصف	الدور المطلوب
GET	`/api/courses`	عرض كل الكورسات	الكل
GET	`/api/courses/{id}`	عرض كورس بالـ ID	الكل
POST	`/api/courses`	إنشاء كورس	Admin
PUT	`/api/courses/{id}`	تعديل كورس	Admin
DELETE	`/api/courses/{id}`	حذف كورس	Admin
Learners — المتعلمون
Method	Endpoint	الوصف
GET	`/api/learners`	عرض كل المتعلمين
GET	`/api/learners/{id}`	عرض متعلم بالـ ID
POST	`/api/learners`	إضافة متعلم
Enrollments — التسجيلات
Method	Endpoint	الوصف	الدور المطلوب
GET	`/api/enrollments`	عرض التسجيلات مع فلترة	الكل
GET	`/api/enrollments/{id}`	عرض تسجيل بالـ ID	الكل
POST	`/api/enrollments`	تسجيل متعلم في كورس	Learner
POST	`/api/enrollments/{id}/decision`	الموافقة أو الرفض	Manager
---
🔍 الفلترة والـ Pagination
```
GET /api/enrollments?learnerId=1
GET /api/enrollments?courseId=2
GET /api/enrollments?status=Approved
GET /api/enrollments?fromDate=2026-01-01&toDate=2026-12-31
GET /api/enrollments?page=2&pageSize=5
```
---
📦 أمثلة على الطلبات
إنشاء كورس (Admin)
```http
POST /api/courses
X-User-Role: Admin
X-User-Id: 1
Content-Type: application/json

{
  "title": "الأمن الرقمي للموظفين الحكوميين",
  "description": "يغطي أفضل ممارسات الأمن السيبراني.",
  "durationHours": 8,
  "requiresApproval": true,
  "isActive": true
}
```
إضافة متعلم
```http
POST /api/learners
Content-Type: application/json

{
  "fullName": "أحمد الراشدي",
  "email": "ahmed@ministry.gov",
  "nationalId": "1234567890",
  "department": "إدارة تقنية المعلومات"
}
```
التسجيل في كورس (Learner)
```http
POST /api/enrollments
X-User-Role: Learner
X-User-Id: 10
Content-Type: application/json

{
  "learnerId": 1,
  "courseId": 1
}
```
الموافقة على تسجيل (Manager)
```http
POST /api/enrollments/1/decision
X-User-Role: Manager
X-User-Id: 5
Content-Type: application/json

{
  "decision": "Approved",
  "reason": "الموظف مستوفٍ لجميع متطلبات الالتحاق"
}
```
رفض تسجيل (Manager)
```http
POST /api/enrollments/1/decision
X-User-Role: Manager
X-User-Id: 5
Content-Type: application/json

{
  "decision": "Rejected",
  "reason": "الموظف لا يستوفي المتطلبات الأساسية"
}
```
---
✅ ملخص قواعد الأعمال
لا يمكن للمتعلم التسجيل في نفس الكورس مرتين (مُطبَّق على مستوى الـ DB والـ Service).
لا يمكن التسجيل في كورس غير نشط.
إذا كان `RequiresApproval = true` → الحالة تبدأ بـ `PendingApproval`.
إذا كان `RequiresApproval = false` → الحالة تكون `Approved` مباشرةً.
فقط التسجيلات بحالة `PendingApproval` يمكن اتخاذ قرار بشأنها.
القرار يجب أن يكون `Approved` أو `Rejected` فقط.
عند الرفض يجب إدخال سبب.
كل قرار يُسجَّل في جدول AuditLogs مع القيم القديمة والجديدة.
---
🛠️ التقنيات المستخدمة
التقنية	الإصدار
ASP.NET Core	8.0
Entity Framework Core	8.0
SQL Server	2019+
Swagger (Swashbuckle)	6.5
FluentValidation	11.3
---
🗄️ مخطط قاعدة البيانات
```
Courses (الكورسات)        Learners (المتعلمون)
──────────────────        ───────────────────
Id (PK)                   Id (PK)
Title                     FullName
Description               Email (UNIQUE)
DurationHours             NationalId (UNIQUE)
RequiresApproval          Department
IsActive                  CreatedAt
CreatedAt

Enrollments (التسجيلات)
───────────────────────
Id (PK)
LearnerId (FK) ──► Learners
CourseId (FK)  ──► Courses
Status
EnrollmentDate
DecisionDate
DecisionReason
DecisionByUserId
UNIQUE(LearnerId, CourseId)

AuditLogs (سجل التدقيق)
────────────────────────
Id (PK)
EntityName
EntityId
Action
OldValue (JSON)
NewValue (JSON)
PerformedBy
PerformedAt
EnrollmentId (FK, nullable) ──► Enrollments
```
