# User Management API

A RESTful API for managing users with CRUD operations, validation, and middleware.

## Features

- **CRUD Operations**: Create, Read, Update, and Delete users
- **Validation Middleware**: Ensures only valid user data is processed
- **Logging Middleware**: Tracks all incoming requests for monitoring
- **Error Handling**: Centralized error handling for better debugging

## Technologies Used

- Node.js
- Express.js
- Body-parser

## Installation

1. Clone the repository:
```bash
git clone <repository-url>
cd rep
```

2. Install dependencies:
```bash
npm install
```

3. Start the server:
```bash
npm start
```

For development with auto-reload:
```bash
npm run dev
```

The server will run on port 3000 by default.

## API Endpoints

### Get All Users
**GET** `/api/users`

Returns a list of all users.

**Response:**
```json
{
  "success": true,
  "count": 2,
  "data": [
    {
      "id": 1,
      "name": "John Doe",
      "email": "john@example.com",
      "age": 30
    },
    {
      "id": 2,
      "name": "Jane Smith",
      "email": "jane@example.com",
      "age": 25
    }
  ]
}
```

### Get User by ID
**GET** `/api/users/:id`

Returns a single user by their ID.

**Response:**
```json
{
  "success": true,
  "data": {
    "id": 1,
    "name": "John Doe",
    "email": "john@example.com",
    "age": 30
  }
}
```

### Create User
**POST** `/api/users`

Creates a new user. Requires `name` and `email` fields. `age` is optional.

**Request Body:**
```json
{
  "name": "Alice Johnson",
  "email": "alice@example.com",
  "age": 28
}
```

**Response:**
```json
{
  "success": true,
  "message": "User created successfully",
  "data": {
    "id": 3,
    "name": "Alice Johnson",
    "email": "alice@example.com",
    "age": 28
  }
}
```

### Update User
**PUT** `/api/users/:id`

Updates an existing user by ID.

**Request Body:**
```json
{
  "name": "Alice Johnson Updated",
  "email": "alice.new@example.com",
  "age": 29
}
```

**Response:**
```json
{
  "success": true,
  "message": "User updated successfully",
  "data": {
    "id": 3,
    "name": "Alice Johnson Updated",
    "email": "alice.new@example.com",
    "age": 29
  }
}
```

### Delete User
**DELETE** `/api/users/:id`

Deletes a user by ID.

**Response:**
```json
{
  "success": true,
  "message": "User deleted successfully",
  "data": {
    "id": 3,
    "name": "Alice Johnson",
    "email": "alice@example.com",
    "age": 28
  }
}
```

## Validation Rules

- **name**: Required, string, minimum 2 characters
- **email**: Required, valid email format
- **age**: Optional, number, between 0 and 150

## Error Handling

The API returns appropriate HTTP status codes and error messages:

- `400 Bad Request`: Invalid input data
- `404 Not Found`: User not found
- `500 Internal Server Error`: Server error

## Project Structure

```
rep/
├── middleware/
│   ├── errorHandler.js
│   ├── loggingMiddleware.js
│   └── validationMiddleware.js
├── models/
│   └── userModel.js
├── routes/
│   └── userRoutes.js
├── package.json
├── server.js
└── README.md
```

## Development Notes

This project was developed with assistance from GitHub Copilot for:
- Code generation and enhancement
- Debugging and error resolution
- Implementing middleware patterns
- Adding validation logic

## License

ISC
