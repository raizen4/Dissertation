const iothub = require('azure-iothub');

const connectionString = 'HostName=Dissertation-IotHub.azure-devices.net;SharedAccessKeyName=iothubowner;SharedAccessKey=OM1nr8BQfYrrUGk9gMEC8P29t0tqBIxgXGvTYgeUMT4=';
const registry = iothub.Registry.fromConnectionString(connectionString);

async function CreateDevice(deviceId) {
  const device = {
    deviceId,
  };
  try {
    const result = await registry.create(device);
    if (result) {
      console.log(result.responseBody.deviceId + result.responseBody.authentication.SymmetricKey);
      const AccountDeviceInfo = {
        ConnectionString: `HostName=Dissertation-IotHub.azure-devices.net;DeviceId=${result.responseBody.deviceId};SharedAccessKey=${result.responseBody.authentication.SymmetricKey}`,
        DeviceId: result.responseBody.deviceId,
        SAK: result.responseBody.authentication.SymmetricKey,
      };

      return AccountDeviceInfo;
    }
    return null;
  } catch (err) {
    console.log(`error: ${err.toString()}`);
    return null;
  }
}

module.exports = { CreateDevice };
