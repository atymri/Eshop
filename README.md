# Eshop

**Eshop** is a simple, customizable platform for creating and managing online stores. It's designed to help developers or small businesses build their own e-commerce system from scratch. Think of it as your own DIY Shopify—lightweight, flexible, and open-source.

-----

## Features

  * **Modular Architecture** – Separate layers for data, business logic, and user interface.
  * **Multi-language Codebase** – Includes C\#, HTML, CSS, and Classic ASP.
  * **E-commerce Ready** – Designed to power product listings, categories, and core transactional flows.
  * **Easy to Extend** – The structure allows for simple addition of new features or custom logic integration.

-----

## Project Structure

```
Eshop/
├── DataLayer/             # Handles database-related code
├── Eshop/                 # Main application logic
├── Eshop.sln              # Visual Studio solution file
├── .gitattributes
└── .gitignore
```

-----

## Getting Started

### Prerequisites

  * **Visual Studio** or **VS Code**
  * **.NET SDK** (required for C\# components)

### Setup

1.  **Clone the Repository**

    ```bash
    git clone https://github.com/atymri/Eshop.git
    cd Eshop
    ```

2.  **Install Dependencies**

    For the .NET components:

    ```bash
    dotnet restore
    ```

3.  **Build the Application**

    For the .NET components:

    ```bash
    dotnet build
    ```

4.  **Run the Application**

    For the .NET components, run the main project:

    ```bash
    dotnet run
    ```

-----

## Contributing

Feel free to fork the project, make changes, and send a pull request. Whether it's a small fix or a big feature, your contributions are welcome.
