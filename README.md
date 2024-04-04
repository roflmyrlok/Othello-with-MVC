# Othello Game API Specification

## Authentication

### Register a New User

- **POST** `/auth/register`
- **Body**:
  - `username`: String
  - `password`: String
- **Success Response**: `201 Created`
- **Error Responses**: `400 Bad Request`, `409 Conflict`

### Login

- **POST** `/auth/login`
- **Body**:
  - `username`: String
  - `password`: String
- **Success Response**: `200 OK`
  - **Body**: `{ "token": "JWT_TOKEN" }`
- **Error Responses**: `400 Bad Request`, `401 Unauthorized`

## Games

### Request a New Game

- **POST** `/games`
- **Headers**:
  - `Authorization`: Bearer JWT_TOKEN
- **Body**:
  - `opponentType`: "player" | "cpu"
- **Success Response**: `202 Accepted`
  - **Body**:
    ```json
    {
      "gameId": String,
      "status": "waiting_for_opponent" | "ready"
    }
    ```
- **Error Responses**: `401 Unauthorized`, `400 Bad Request`

### Get Game Field

- **GET** `/games/{gameId}`
- **Headers**:
  - `Authorization`: Bearer JWT_TOKEN
- **Success Response**: `200 OK`
  - **Body**:
    ```json
    {
      "gameId": String,
      "playerOne": String,
      "playerTwo": String,
      "status": "waiting" | "in_progress" | "completed",
      "winner": CellState,
      "field": Field
    }
    ```
- **Error Responses**: `401 Unauthorized`, `404 Not Found`

### Get Hint

- **GET** `/games/{gameId}/hint`
- **Headers**:
  - `Authorization`: Bearer JWT_TOKEN
- **Success Response**: `200 OK`
  - **Body**:
    ```json
    {
      "gameId": String,
      "playerOne": String,
      "playerTwo": String,
      "status": "waiting" | "in_progress" | "completed",
      "winner": CellState,
      "field": Field,
      "mask": List<bool,bool>
    }
    ```
- **Error Responses**: `401 Unauthorized`, `404 Not Found`

### Make a Move

- **POST** `/games/{gameId}/move`
- **Headers**:
  - `Authorization`: Bearer JWT_TOKEN
- **Body**:
  - `x`: Number
  - `y`: Number
- **Success Response**: `200 OK`
- **Error Responses**: `401 Unauthorized`, `400 Bad Request`, `404 Not Found`

### Make a Undo

- **POST** `/games/{gameId}/undo`
- **Headers**:
  - `Authorization`: Bearer JWT_TOKEN
- **Body**:
- **Success Response**: `200 OK`
- **Error Responses**: `401 Unauthorized`, `400 Bad Request`, `404 Not Found`

## Users

### Get User Stats

- **GET** `/users/{username}/stats`
- **Headers**:
  - `Authorization`: Bearer JWT_TOKEN
- **Description**: Retrieves the total number of games played, wins, and losses for the user.
- **Success Response**: `200 OK`
  - **Body**:
    ```json
    {
      "totalGames": Number,
      "wins": Number,
      "losses": Number
    }
    ```
- **Error Responses**: `401 Unauthorized`, `404 Not Found`

### Get User Game History

- **GET** `/users/{username}/games`
- **Headers**:
  - `Authorization`: Bearer JWT_TOKEN
- **Description**: Retrieves a list of games played by the user, including game IDs, opponents, outcomes, and dates.
- **Success Response**: `200 OK`
  - **Body**:
    ```json
    [
      {
        "gameId": String,
        "opponent": String,
        "outcome": "win" | "loss" | "draw",
        "playedOn": String
      }
    ]
    ```
- **Error Responses**: `401 Unauthorized`, `404 Not Found`

## Communication

### Send a Message

- **POST** `/games/{gameId}/react`
- **Headers**:
  - `Authorization`: Bearer JWT_TOKEN
- **Body**:
  - `emote`: Emote
- **Success Response**: `201 Created`
- **Error Responses**: `401 Unauthorized`, `400 Bad Request`, `404 Not Found`

### Get Emotes

- **GET** `/games/{gameId}/emotes`
- **Headers**:
  - `Authorization`: Bearer JWT_TOKEN
- **Success Response**: `200 OK`
  - **Body**:
   ```json
    [
      {
        "emote": List<Emote>
      }
    ]
    ```
- **Error Responses**: `401 Unauthorized`, `404 Not Found`
