const { I } = inject();
const expect = require('chai').expect;

Feature('Media API');

let createdMediaId;

Scenario('Create, Read, Update, and Delete Media', async () => {
  // Create Media
  const createPayload = {
    url: 'https://example.com/image.jpg',
    productId: '00000000-0000-0000-0000-000000000001' // Replace with a valid product ID
  };

  const createResponse = await I.sendPostRequest('/api/media', createPayload);
  expect(createResponse.status).to.equal(201);
  expect(createResponse.data.isSuccess).to.be.true;
  expect(createResponse.data.message).to.equal('Media created successfully');
  createdMediaId = createResponse.data.result.id;

  // Read Media
  const getResponse = await I.sendGetRequest(`/api/media/${createdMediaId}`);
  expect(getResponse.status).to.equal(200);
  expect(getResponse.data.isSuccess).to.be.true;
  expect(getResponse.data.result.url).to.equal(createPayload.url);
  expect(getResponse.data.result.productId).to.equal(createPayload.productId);

  // Update Media
  const updatePayload = {
    url: 'https://example.com/updated-image.jpg',
    productId: '00000000-0000-0000-0000-000000000002' // Replace with another valid product ID
  };

  const updateResponse = await I.sendPutRequest(`/api/media/${createdMediaId}`, updatePayload);
  expect(updateResponse.status).to.equal(200);
  expect(updateResponse.data.isSuccess).to.be.true;
  expect(updateResponse.data.message).to.equal('Media updated successfully');

  // Verify Update
  const getUpdatedResponse = await I.sendGetRequest(`/api/media/${createdMediaId}`);
  expect(getUpdatedResponse.status).to.equal(200);
  expect(getUpdatedResponse.data.isSuccess).to.be.true;
  expect(getUpdatedResponse.data.result.url).to.equal(updatePayload.url);
  expect(getUpdatedResponse.data.result.productId).to.equal(updatePayload.productId);

  // Delete Media
  const deleteResponse = await I.sendDeleteRequest(`/api/media/${createdMediaId}`);
  expect(deleteResponse.status).to.equal(200);
  expect(deleteResponse.data.isSuccess).to.be.true;
  expect(deleteResponse.data.message).to.equal('Media deleted successfully');

  // Verify Deletion
  const getDeletedResponse = await I.sendGetRequest(`/api/media/${createdMediaId}`);
  expect(getDeletedResponse.status).to.equal(404);
  expect(getDeletedResponse.data.isSuccess).to.be.false;
  expect(getDeletedResponse.data.message).to.equal('Media not found');
});

Scenario('Try to update non-existent Media', async () => {
  const nonExistentId = '00000000-0000-0000-0000-000000000099'; // A non-existent ID
  const updatePayload = {
    url: 'https://example.com/non-existent.jpg',
    productId: '00000000-0000-0000-0000-000000000001' // Replace with a valid product ID
  };

  const updateResponse = await I.sendPutRequest(`/api/media/${nonExistentId}`, updatePayload);
  expect(updateResponse.status).to.equal(404);
  expect(updateResponse.data.isSuccess).to.be.false;
  expect(updateResponse.data.message).to.equal('Media not found');
});
