# Use an official Node.js runtime as the base image
FROM node:14-alpine

# Set the working directory in the container
WORKDIR /app

# Copy package.json and package-lock.json (if available)
COPY package*.json ./

# Install project dependencies
RUN npm install

# Copy the entire project directory into the container
COPY . .

# Build the React app
RUN npm run build

# Set environment variables (if necessary)
# ENV REACT_APP_API_URL=http://api.example.com

# Expose a port (if your app listens on a specific port)
EXPOSE 3000

# Start the React app
CMD ["npm", "start"]
