/* eslint-disable no-undef */

const BaseResponse = require('../serviceModels/BaseResponse');

// eslint-disable-next-line no-unused-vars
class ResponseData extends BaseResponse {
  constructor() {
    super();

    this.Content = null;
  }
}
module.exports = ResponseData;
