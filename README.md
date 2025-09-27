# Helsi Test Task

## How to Launch

1. **Start MongoDB using Docker**  
   Make sure Docker is running, then execute the following command:  
   ```bash
    docker run -d --name my-mongo -p 27017:27017 \
    -e MONGO_INITDB_DATABASE=task_lists \
    -e MONGO_INITDB_ROOT_USERNAME=admin \
    -e MONGO_INITDB_ROOT_PASSWORD=secret \
    mongo:7
   ```

2. **Run the Project**  
   Once the MongoDB container is up, open the project in any IDE and run it.  
   The settings already contain the correct database name and connection string.  

> ⚠️ **Note:** Storing credentials in `appsettings.json` is only for convenience to quickly start the project. In a real project, sensitive data should **never** be stored in JSON files or public repositories.

---

## Authentication / User ID

- Authentication and authorization are **not implemented**, as it was not required for the test task.  
- The user ID for requests is provided via `FromHeader`.  
- In a real-world scenario, claims-based authentication would be recommended.

---

## Notes / Possible Optimizations

- Currently, access to a TaskList is checked by iterating through each list and verifying `SharedWithUsersIds`.  
- A more optimized approach would be to create a `Users` collection storing a `HashSet` of accessible TaskList IDs for each user.  
  - This would improve performance when checking access.  
  - Updating access would require MongoDB transactions to ensure data consistency.  
