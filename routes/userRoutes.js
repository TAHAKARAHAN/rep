const express = require('express');
const router = express.Router();
const userModel = require('../models/userModel');
const validationMiddleware = require('../middleware/validationMiddleware');

// GET /api/users - Get all users
router.get('/', (req, res) => {
  try {
    const users = userModel.getAllUsers();
    res.json({
      success: true,
      count: users.length,
      data: users
    });
  } catch (error) {
    res.status(500).json({
      success: false,
      error: 'Failed to retrieve users'
    });
  }
});

// GET /api/users/:id - Get user by ID
router.get('/:id', (req, res) => {
  try {
    const userId = parseInt(req.params.id);
    const user = userModel.getUserById(userId);

    if (!user) {
      return res.status(404).json({
        success: false,
        error: 'User not found'
      });
    }

    res.json({
      success: true,
      data: user
    });
  } catch (error) {
    res.status(500).json({
      success: false,
      error: 'Failed to retrieve user'
    });
  }
});

// POST /api/users - Create new user
router.post('/', validationMiddleware, (req, res) => {
  try {
    const newUser = userModel.createUser(req.body);
    res.status(201).json({
      success: true,
      message: 'User created successfully',
      data: newUser
    });
  } catch (error) {
    res.status(500).json({
      success: false,
      error: 'Failed to create user'
    });
  }
});

// PUT /api/users/:id - Update user
router.put('/:id', validationMiddleware, (req, res) => {
  try {
    const userId = parseInt(req.params.id);
    const updatedUser = userModel.updateUser(userId, req.body);

    if (!updatedUser) {
      return res.status(404).json({
        success: false,
        error: 'User not found'
      });
    }

    res.json({
      success: true,
      message: 'User updated successfully',
      data: updatedUser
    });
  } catch (error) {
    res.status(500).json({
      success: false,
      error: 'Failed to update user'
    });
  }
});

// DELETE /api/users/:id - Delete user
router.delete('/:id', (req, res) => {
  try {
    const userId = parseInt(req.params.id);
    const deletedUser = userModel.deleteUser(userId);

    if (!deletedUser) {
      return res.status(404).json({
        success: false,
        error: 'User not found'
      });
    }

    res.json({
      success: true,
      message: 'User deleted successfully',
      data: deletedUser
    });
  } catch (error) {
    res.status(500).json({
      success: false,
      error: 'Failed to delete user'
    });
  }
});

module.exports = router;
