module.exports = {
    "extends": "airbnb-base",
    "rules":{
        "linebreak-style":0,
        "func-names":0,
        "indent":["error",2],
        "complexity": ["warning", 4],
        "complexity": ["error", 7],
        "max-lines": ["error", 250],
        
    },
    "env":{
    "es6": true,
    "jasmine": true,
    "node": true,       
    "browser": true,
    "jest": true
    },
     "parserOptions": {
    "ecmaVersion": 8
    },
    "overrides": [{
		"files": [ "*.test.js" ],
		"rules": {
			"max-lines-per-function": "off",
			"max-lines": "off",
			"no-magic-numbers": "off"
		}
	}]
};