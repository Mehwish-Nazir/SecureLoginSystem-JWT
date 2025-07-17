#  SecureLoginSystem-JWT

A full-stack authentication system using **Angular (Frontend)** and **ASP.NET Core Web API (Backend)** implementing **JWT (JSON Web Token)** based authentication with secure login for role based API protection.

---

##  Project Structure
FullStackAuthentication/
├── FrontEnd/
│ └── AuthenticationFrontEnd (Angular 17 App)
├── BackEnd/
│ └── AuthenticationBackEnd (.NET 8 API)

---

##  Features
- User first Registered 
-  User login with JWT authentication
-  Unauthorized access handling
-  Protected API endpoints
-  Frontend-backend folder separation
-  Login success and failure flow with proper messages
-  Token stored securely on the client side

---

## 🛠️ Technologies Used

### Frontend:
- Angular 17
  
- Angular HTTP Interceptors

### Backend:
- ASP.NET Core 8 Web API
- JWT Bearer Authentication
- Entity Framework Core 
- C#

---

##  Getting Started

### Prerequisites:
- Node.js & Angular CLI
- .NET SDK 8.0
- Git
- SSMS Server

###  Backend Setup (AuthenticationBackEnd)

```bash
cd BackEnd/AuthenticationBackEnd
dotnet restore
dotnet build
dotnet run
---

### Frontend Setup (AuthenticationFrontEnd)

cd FrontEnd/AuthenticationFrontEnd
npm install
ng serv

---

###  JWT Authentication Flow
User logs in from Angular frontend.

Credentials are sent to .NET API.

If valid, API returns a JWT token.

Token is stored on the client.

All further requests send token via HTTP headers.

----

###ScreenShots

###  Successful Register

<img width="782" height="284" alt="Register" src="https://github.com/user-attachments/assets/567ab23d-c398-4d2d-8584-e5b03e16ec81" />

###SuccessFull Login With Jwt Token Generation
<img width="782" height="284" alt="Login" src="https://github.com/user-attachments/assets/9bc7d673-6ab2-4bd3-8e85-e1c99ea5257d" />

###Error Handling During Invalid Password Lenght or duplicate emails

<img width="1823" height="838" alt="Screenshot 2025-07-17 232826" src="https://github.com/user-attachments/assets/2ee266d9-4698-4d5d-94e1-9f7a9f64c30f" />
<img width="1763" height="811" alt="Screenshot 2025-07-17 232726" src="https://github.com/user-attachments/assets/f4c1823e-41af-4b2f-8925-a6f3e11490dc" />
 






