# 2FAApi
# Two-Factor Authentication Service

This is a simple Two-Factor Authentication (2FA) service built using C# and .NET 6. The service provides two API endpoints for sending and verifying confirmation codes via SMS. It allows users to enhance the security of their accounts by requiring a second factor (a code sent to their registered phone number) for authentication.

## Features

- **Send Confirmation Code**: Send a confirmation code to a specified phone number.

- **Verify Confirmation Code**: Verify a received confirmation code against the phone number to confirm authentication.

- **Configurable Code Lifetime**: You can configure how long a confirmation code remains valid.

- **Limit Concurrent Codes**: You can specify the maximum number of concurrent codes per phone number.

## Getting Started

1. Clone the repository to your local machine.
2. Install .NET 6
3. Build and Run the application in any IDE
