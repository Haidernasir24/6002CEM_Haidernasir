const functions = require("firebase-functions");
const crypto = require("crypto");
const admin = require("firebase-admin");
admin.initializeApp();
exports.sendMessage = functions.https.onRequest(async (req, res) => {
  const {userId, text, recipientId} = req.body;
  const timestamp = Date.now();
  const characters = `ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz
  0123456789`;
  let result = "";
  const charactersLength = characters.length;
  for (let i = 0; i < 12; i++) {
    const randomByte = crypto.randomBytes(1)[0];
    result += characters[randomByte % charactersLength];
  }
  const msgRef = admin.database().ref(`messages/${recipientId}`);
  await msgRef.set({
    [result]: {
      text,
      userId,
      timestamp,
    },
  });
});
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
    const msgRef = admin.database().ref(`messages/${userRecord.uid}`);
    const userRef = admin.database().ref(`users/${userRecord.uid}`);
    await userRef.set({
      displayName,
    });
    await msgRef.set({
      ignore: "persist",
    });
    res.status(201).send({userId: userRecord.uid});
  } catch (error) {
    console.error("Error creating new user:", error);
    res.status(500).send("Error creating user");
  }
});

