// Validation middleware to ensure only valid user data is processed
const validationMiddleware = (req, res, next) => {
  const { name, email, age } = req.body;

  // Validate required fields
  if (!name || !email) {
    return res.status(400).json({
      error: 'Missing required fields',
      message: 'Name and email are required'
    });
  }

  // Validate name format
  if (typeof name !== 'string' || name.trim().length < 2) {
    return res.status(400).json({
      error: 'Invalid name',
      message: 'Name must be a string with at least 2 characters'
    });
  }

  // Validate email format
  const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
  if (!emailRegex.test(email)) {
    return res.status(400).json({
      error: 'Invalid email',
      message: 'Email must be a valid email address'
    });
  }

  // Validate age if provided
  if (age !== undefined) {
    if (typeof age !== 'number' || age < 0 || age > 150) {
      return res.status(400).json({
        error: 'Invalid age',
        message: 'Age must be a number between 0 and 150'
      });
    }
  }

  next();
};

module.exports = validationMiddleware;
