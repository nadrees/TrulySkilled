var mongoose = require('mongoose'),
    Schema = mongoose.Schema,
    ObjectId = Schema.Types.ObjectId;

/*************************
******* Schemas **********
*************************/
var userSchema = new Schema({
    googleToken: { type: String, unique: true },
    displayName: { type: String, required: true }
});

/************************
******* Models **********
************************/
var User = mongoose.model('User', userSchema);

module.exports = {
    User: User
};