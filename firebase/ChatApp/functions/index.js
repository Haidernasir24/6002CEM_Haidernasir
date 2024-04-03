const functions = require("firebase-functions");
const admin = require("firebase-admin");
admin.initializeApp();

exports.createUser = functions.https.onRequest(async (req, res) => {
  if (req.method !== "POST") {
    return res.status(405).send("Method Not Allowed");
  }
  const {email, password, displayName} = req.body;
  if (!email || !password || !displayName) {
    return res.status(400).send(`Email, password and display name are
     required`);
  }
  try {
    const userRecord = await admin.auth().createUser({
      email,
      password,
      displayName,
    });
    res.status(201).send({userId: userRecord.uid});
  } catch (error) {
    console.error("Error creating new user:", error);
    res.status(500).send("Error creating user");
  }
});

