# Use the official Python image
FROM python:3.12-slim

# Set the working directory in the container
WORKDIR /app

# Copy the requirements file
COPY requirements.txt .

# Install dependencies
RUN pip install --no-cache-dir -r requirements.txt

# Copy the current directory contents into the container at /app
COPY . .

# Copy certificates into the container
COPY certificates/ /app/certificates/

# Expose port 8088 for the application
EXPOSE 8088

# Run the application
CMD ["python", "main.py"]
