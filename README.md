## README.md

# Number to Words Converter Web Application

This web application converts numerical input into words and displays the output as a string parameter. The application consists of a frontend built with React and Vite, and a backend implemented using ASP.NET Core 8.0 with an in-memory database.

## Table of Contents

1. [Project Description](#project-description)
2. [Features](#features)
3. [Technologies Used](#technologies-used)
4. [Installation](#installation)
5. [Running the Application](#running-the-application)
6. [Testing](#testing)
7. [Project Structure](#project-structure)
8. [Approach and Design Decisions](#approach-and-design-decisions)
9. [Contact Information](#contact-information)

## Project Description

This project is designed to convert numerical input into words, such as converting "123.45" into "ONE HUNDRED AND TWENTY-THREE DOLLARS AND FORTY-FIVE CENTS". The solution includes an interactive web user interface to test the conversion routine.

## Features

- Convert numerical input to words.
- Interactive web interface.
- Validation for input correctness.
- Error handling for invalid input and API errors.

## Technologies Used

### Frontend
- **React**
- **Vite**
- **Vitest with Testing Library**: For writing tests.

### Backend
- **ASP.NET Core 8.0**
- **In-memory Database**
- **xUnit**: A testing framework for .NET.

## Installation

### Prerequisites

- Node.js and npm
- .NET SDK 8.0

### Frontend Setup

1. Navigate to the front repository:
    ```bash
    cd ConverDigits/ClientApp
    ```

2. Install the dependencies:
    ```bash
    npm install
    ```

### Backend Setup

1. Navigate to the backend directory:
    ```bash
    cd ConvertDigits
    ```

2. Restore the .NET dependencies:
    ```bash
    dotnet restore
    ```

## Running the Application

### Frontend

1. Start the development server:
    ```bash
    npm run dev
    ```

2. Open your browser and navigate to `http://localhost:5173`.

### Backend

1. Start the backend server:
    ```bash
    dotnet run
    ```

2. The backend server will run on `http://localhost:5218`.

## Testing

### Frontend

1. Run the tests:
    ```bash
    npm run test
    ```

### Backend

1. Run the tests:
    ```bash
    dotnet test
    ```

## Project Structure

```
.
├── ClientApp
│   ├── public
│   ├── src
│   │   ├── assets
│   │   ├── tests
│   │   │   ├── setup.js
│   │   ├── App.css
│   │   ├── App.jsx
│   │   ├── App.test.jsx
│   │   ├── index.css
│   │   ├── main.jsx
│   ├── .eslintrc.cjs
│   ├── index.html
│   ├── package.json
│   ├── vite.config.js
├── Controllers
    ├── ConvertController.cs
├── Models
    ├── NumberContext.cs
    ├── Numbers.cs
├── Properties
├── Test
    ├── ConvertDigits.Tests.cs
├── Program.cs
├── ConvertDigits.csproj
├── ConvertDigits.sln
├── ConvertDigits.http
├── appsettings.Development.json
├── appsettings.json
├── .gitignore
├── Test-Plan
```

## API Documentation

### Convert Number to Words

- **URL**: `/api/convert`
- **Method**: `POST`
- **Request Body**: 
    ```json
    {
        "number": "123.45"
    }
    ```
- **Response**: 
    ```json
    {
        "words": "ONE HUNDRED AND TWENTY-THREE DOLLARS AND FORTY-FIVE CENTS"
    }
    ```

## Approach and Design Decisions

### Approach

1. **Frontend**: Chose React for its component-based architecture and Vite for its fast development environment.
2. **Backend**: Used ASP.NET Core for its robust framework and in-memory database for simplicity during development.
3. **Testing**: Implemented tests using Vitest + Testing Library for the frontend and xUnit for the backend to ensure code quality and reliability.

### Design Decisions

1. **Custom Algorithm**: Developed a custom algorithm for converting numbers to words instead of using existing libraries to demonstrate unique problem-solving skills.
2. **Validation**: Added validation to handle various edge cases and ensure input correctness.
3. **Error Handling**: Implemented error handling to provide meaningful messages to users in case of invalid input or server errors.

## Contact Information

- **Author**: Jaeseong Jeong
- **Email**: [js104427@gmail.com](mailto:js104427@gmail.com)

---

Please feel free to reach out if you have any questions or need further assistance.

