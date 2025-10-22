# EmailBuilder App

A full-stack solution for designing and sending emails using a React frontend (with Unlayer Email Editor) and an ASP.NET Core backend with SendGrid integration.

---

## Features
- Design emails visually with Unlayer in React
- Enter recipient, subject, CC, BCC, and add attachments
- Send emails via a secure ASP.NET Core API using SendGrid
- Rate limiting for API protection

---

## Prerequisites
- Node.js (v18+ recommended)
- .NET 8 or 9 SDK
- Free SendGrid account ([signup](https://sendgrid.com))

---

## Backend Setup (ASP.NET Core API)

1. **Clone the repo and navigate to backend:**
   ```sh
   git clone <your-repo-url>
   cd EmailBuilder/backend
   ```
2. **Configure SendGrid:**
   - Open `src/EmailBuilderApi.Api/appsettings.Development.json`.
   - Add your SendGrid API key, from email, and from name:
     ```json
     "SendGrid": {
       "ApiKey": "YOUR_SENDGRID_API_KEY",
       "FromEmail": "your@email.com",
       "FromName": "Your Name"
     }
     ```
3. **Build and run the API:**
   ```sh
   dotnet build
   dotnet run --project src/EmailBuilderApi.Api/EmailBuilderApi.Api.csproj
   ```
   - The API will run on `http://localhost:5034` by default.

---

## Frontend Setup (React App)

1. **Navigate to frontend:**
   ```sh
   cd ../frontend
   ```
2. **Install dependencies:**
   ```sh
   npm install
   ```
3. **Start the React app:**
   ```sh
   npm start
   ```
   - The app will open at `http://localhost:3000`.

---

## Sending Emails from the App
- Fill in recipient, subject, (optional) CC/BCC, and add attachments if needed.
- Design your email with the Unlayer editor.
- Click **Use This Design** to export the HTML.
- Click **Send Email** to send via the backend API and SendGrid.

---

## Using Postman to Test the API

1. **Endpoint:**
   - `POST http://localhost:5034/api/email/send`
2. **Content-Type:**
   - `multipart/form-data`
3. **Body Parameters:**
   - `recipient` (string, required)
   - `subject` (string, required)
   - `htmlContent` (string, required)
   - `cc` (string, optional, comma-separated)
   - `bcc` (string, optional, comma-separated)
   - `attachments` (file, optional, can send multiple)
4. **Example:**
   - Add fields in Postman form-data tab, attach files as needed.
   - Click **Send** to test email delivery.

---

## Notes
- You must verify sender and recipient emails in SendGrid if using a free/trial account.
- Check your spam folder if emails do not appear in your inbox.
- Rate limiting is enabled (100 requests/minute per IP).

---

## Troubleshooting
- **API errors:** Check API logs and your SendGrid configuration.
- **Frontend errors:** Check browser console and ensure API is running.
- **Email not received:** Check spam, verify SendGrid sender, and ensure correct API key.

---
