# PixelBlog

An interactive blog/news platform powered by **ASP.NET Core 9**, featuring a gamified experience system and dynamic content handling.

> ⚙️ This project was developed over **3 weeks** as a full-stack backend-focused build, with ongoing frontend implementation. Everything from logic design, entity mapping, XP system, to Swagger testing is crafted with learning and scalability in mind.
---
> ⚙️ Frontend (Next.js) development in progress — backend is complete and production-ready.

---

## ✅ Completed Features (As of May 13, 2025)

### **Core Backend:**

* ✅ ASP.NET Core 9 + PostgreSQL setup
* ✅ Entity Creation: User, Blog, BlogReadLog, BlogLike, BlogSave
* ✅ DTOs: BlogDto, BlogCreateDto, BlogUpdateDto, UserDto, UserLoginDto, UserRegisterDto
* ✅ AutoMapper setup
* ✅ JWT Authentication & Authorization
* ✅ ClaimsPrincipalExtensions with GetUserId()

### **Blog System:**

* ✅ Blog CRUD operations
* ✅ +30 XP on blog creation (via BlogService)
* ✅ +10 XP on reading other users’ blogs (ReadBlogById)
* ✅ Blog view count increases only if reader is not the owner
* ✅ BlogUser relationship
* ✅ GetMyBlogs endpoint
* ✅ Multi-image support added (`ImageUrls: List<string>?`)
* ✅ BlogReadLog entity & table created to track which users read which blogs
* ✅ XP and view count only incremented once per user per blog

### **User System:**

* ✅ Register/Login (with HMACSHA512 hashing)
* ✅ Default Role = User
* ✅ GetCurrentUser endpoint
* ✅ BlogCount field
* ✅ Level + XP System integrated

### **Save & Like System (Completed May 10–13, 2025):**

* ✅ BlogLike & BlogSave entities created
* ✅ Composite keys configured in DataContext.cs
* ✅ Generic `ToggleRelationAsync<TEntity>` helper implemented
* ✅ ToggleLike and ToggleSave methods in BlogService
* ✅ LikeCount & SaveCount tracked and reflected in BlogDto
* ✅ `IsLikedByCurrentUser` and `IsSavedByCurrentUser` flags
* ✅ Endpoints:

  * `/api/blogs/liked`
  * `/api/blogs/saved`
  * `/api/blogs/liked-and-saved`

### **Refactor & Improvements:**

* ✅ XP, level, and blog logic moved to BlogService
* ✅ Controller cleanup (BlogController delegates logic to BlogService)
* ✅ Removed duplicated logic (UpdateUserExperience now centralized)
* ✅ FluentValidation added for UserRegisterDto, UserLoginDto, BlogCreateDto, BlogUpdateDto
* ✅ Password complexity enforced via regex (uppercase, number, special char)

### **Tools:**

* ✅ Swagger tested with auth
* ✅ Postman tested

### **Blog Read Log (Completed May 7, 2025):**

* ✅ BlogReadLog table
* ✅ Prevent multiple XP gains & view bumps per user
* ❌ Optional: Reader XP +10, Blog Author Passive XP +5 (not yet)

---

## 📌 Next Focus (Starting May 13, 2025):

* 🚧 Start frontend development (Next.js)
* 🧩 Design system planning
* 🎨 Blog card layout with like/save icons and counts
* 💾 Public blog view
* 🔁 Pagination support in frontend
* 🧪 Test backend integration with frontend

---

## 🧠 Notes:

* Like/Save logic handled completely in BlogService
* Controller layer is minimal and clean
* NotificationService to be added later
* Pagination planned in both API and frontend

---

## 📝 Dev Recap (May 5–12, 2025):

* ✅ FluentValidation for all major DTOs
* ✅ Password regex validation
* ✅ BlogService abstraction
* ✅ XP/View logic in ReadBlogByIdAsync
* ✅ Controllers refactored
* ✅ ImageUrls with 10-image limit
* ✅ BlogReadLog logic finalized
* ✅ ToggleLike/Save logic with proper counts
* ✅ Combined endpoint for liked + saved blogs
* ✅ Obsidian documentation kept in sync

---

## ✨ Suggested & Future Features

### **3. Badges & Achievements**

* XP/badge tiers (Bronze–Gold titles)

### **4. Public Blog View**

* Read-only blog access without login

### **5. Tags & Filtering**

* Add tags to blogs
* Filter by tag (#dotnet, #frontend)

### **6. Comments**

* Nested replies
* SignalR (optional)
* Reactions on comments

### **7. Forum System**

* Q\&A style community
* XP/badges
* Moderation tools

### **8. Notifications**

* Blog liked/commented notifications
* Initially non-realtime (future: SignalR)

### **9. Blog Drafts**

* Save blog as draft
* View/manage draft list

### **10. Follow System**

* Follow users
* Feed from followed users

### **11. Blog Series**

* Link blogs (Part 1, 2, ...)
* Show “Next in Series”

### **12. Rich Content + Slider**

* BlogImages table
* Swiper.js support for gallery
* Mixed text-image blocks

### **13. ContentType Field**

* Blog / News / Guide types
* Filter or visually distinguish types

### **14. Redis Integration**

* Cache hot data (views, likes)
* Real-time dashboard support

### **15. XP From Likes**

* +5 XP per 10 likes
* Max 1 like per user
* Optional XP limit per day

### **16. Like Milestone Notifications**

* Notify author at 5/10/20 likes
* Via NotificationService

### **17. Pagination Support (Backend + Frontend)**

* Backend: `Skip` + `Take`
* Frontend: `/api/blogs?page=2&pageSize=10`
* UI: Button-based or infinite scroll

---

🔮 **AI-Based Blog Recommendation**

* Track blog interactions
* ML-based recommendations (Python or ML.NET)
* Serve recommendations via API

---



**XP never dies. PixelBlog lives on. 🚀**
