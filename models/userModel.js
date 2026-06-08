// In-memory user data storage
let users = [
  {
    id: 1,
    name: 'John Doe',
    email: 'john@example.com',
    age: 30
  },
  {
    id: 2,
    name: 'Jane Smith',
    email: 'jane@example.com',
    age: 25
  }
];

let nextId = 3;

// Get all users
const getAllUsers = () => {
  return users;
};

// Get user by ID
const getUserById = (id) => {
  return users.find(user => user.id === id);
};

// Create new user
const createUser = (userData) => {
  const newUser = {
    id: nextId++,
    ...userData
  };
  users.push(newUser);
  return newUser;
};

// Update user
const updateUser = (id, userData) => {
  const index = users.findIndex(user => user.id === id);
  if (index === -1) {
    return null;
  }
  users[index] = { ...users[index], ...userData };
  return users[index];
};

// Delete user
const deleteUser = (id) => {
  const index = users.findIndex(user => user.id === id);
  if (index === -1) {
    return null;
  }
  const deletedUser = users.splice(index, 1)[0];
  return deletedUser;
};

module.exports = {
  getAllUsers,
  getUserById,
  createUser,
  updateUser,
  deleteUser
};
