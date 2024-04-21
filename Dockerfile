# Use the official SQL Server 2019 image from Docker Hub
FROM mcr.microsoft.com/mssql/server:2019-latest

# Set environment variables
ENV ACCEPT_EULA=Y \
    SA_PASSWORD=E6rLVj7sHW8v2hx

# Create a directory for SQL Server data
RUN mkdir -p /var/opt/mssql/data

# Set the working directory
WORKDIR /var/opt/mssql/data

# Copy the SQL Server scripts to the container
COPY ./scripts/ .

# Grant permissions to the scripts if any files are present
RUN ls -A1 | xargs -I {} chmod +x {}

# Start SQL Server when the container starts
CMD /opt/mssql/bin/sqlservr
