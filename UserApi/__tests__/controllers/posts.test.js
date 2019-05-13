/* eslint-disable prefer-destructuring */
/* eslint-disable no-unused-vars */
const sinon = require('sinon');
const request = require('supertest');
const Post = require('../../DbSchemas/PostsSchema');
const PostsManager = require('../../manager/PostsManager');
const server = require('../../app').server;
const PostDto = require('../../serviceModels/PostDto');

describe('Posts db tests', () => {
  it('should hit GetAllPosts and return 200 OK with some posts', async () => {
    const responseFromManager = [];
    responseFromManager.push(new Post({
      Content: 'test content',
      Title: 'test content',
      AuthorId: '12345678899899',
      AuthorName: 'test author name',
      IsVisible: false,
      _id: '22222222222',
    }),
    new Post({
      Content: 'test content 2',
      Title: 'test content 2',
      AuthorId: '12345678899899',
      _id: '11111111111',
      AuthorName: 'test author name 2',
      IsVisible: false,

    }));

    const myStub = sinon.stub(PostsManager, 'GetAllPosts').returns(responseFromManager);
    const response = await request(server)
      .get('/PostsApi/posts/GetAllPosts').expect(200);
    expect(response.body.HasBeenSuccessful).toBe(true);
    expect(response.body.Content[0].Content).toBe('test content');
    expect(response.body.Errors).toBe(null);
    myStub.restore();
  });

  it('should hit GetAllPosts and return 200 OK with no posts', async () => {
    const myStub = sinon.stub(PostsManager, 'GetAllPosts').returns(null);
    const response = await request(server)
      .get('/PostsApi/posts/GetAllPosts').expect(200);
    expect(response.body.HasBeenSuccessful).toBe(false);
    expect(response.body.Content).toBe(null);
    expect(response.body.Errors).toBe('Api internal error-Manager');
    myStub.restore();
  });

  it('should hit GetAllPosts and throw an error', async () => {
    const myStub = sinon.stub(PostsManager, 'GetAllPosts').throws(new Error('testing catch'));
    const response = await request(server)
      .get('/PostsApi/posts/GetAllPosts').expect(200);
    expect(response.body.HasBeenSuccessful).toBe(false);
    expect(response.body.Content).toBe(null);
    expect(response.body.Errors).toBe('Api internal error-Manager');
    myStub.restore();
  });

  it('should hit GetPostForUser and return 200 OK with some posts', async () => {
    const responseFromManager = [];
    const notStringifiedUser = {
      Name: 'testUser',
      Username: 'mockedUser',
      Id: '1245566',
      DisplayName: 'testUser',
    };
    const stringifiedUser = JSON.stringify({
      Name: 'testUser',
      Username: 'mockedUser',
      Id: '1245566',
      DisplayName: 'testUser',
    });
    responseFromManager.push(new Post({
      Content: 'test content',
      Title: 'test content',
      AuthorId: '12345678899899',
      AuthorName: 'test author name',
      IsVisible: false,
      _id: '22222222222',
    }),
    new Post({
      Content: 'test content 2',
      Title: 'test content 2',
      AuthorId: '12345678899899',
      _id: '11111111111',
      AuthorName: 'test author name 2',
      IsVisible: false,

    }));

    const myStub = sinon.stub(PostsManager, 'GetPostsForUser').returns(responseFromManager);

    const response = await request(server)
      .get('/PostsApi/posts/GetAllPostsForCurrentUser').send(stringifiedUser).expect(200);
    const stubCalledCorrectly = myStub.calledWith(notStringifiedUser.Id);
    expect(stubCalledCorrectly).toBe(true);
    expect(response.body.HasBeenSuccessful).toBe(true);
    expect(response.body.Content[0].Content).toBe('test content');
    expect(response.body.Errors).toBe(null);
    myStub.restore();
  });

  it('should hit GetPostForUser and return 200 OK with no posts', async () => {
    const notStringifiedUser = {
      Name: 'testUser',
      Username: 'mockedUser',
      Id: '1245566',
      DisplayName: 'testUser',
    };
    const stringifiedUser = JSON.stringify({
      Name: 'testUser',
      Username: 'mockedUser',
      Id: '1245566',
      DisplayName: 'testUser',
    });
    const myStub = sinon.stub(PostsManager, 'GetPostsForUser').returns(null);
    const response = await request(server)
      .get('/PostsApi/posts/GetAllPostsForCurrentUser').send(stringifiedUser).expect(200);
    const stubCalledCorrectly = myStub.calledWith(notStringifiedUser.Id);
    expect(stubCalledCorrectly).toBe(true);
    expect(response.body.HasBeenSuccessful).toBe(false);
    expect(response.body.Content).toBe(null);
    expect(response.body.Errors).toBe('Api internal error-Manager');
    myStub.restore();
  });

  it('should hit GetPostForUser and return in catch', async () => {
    const notStringifiedUser = {
      Name: 'testUser',
      Username: 'mockedUser',
      Id: '1245566',
      DisplayName: 'testUser',
    };
    const stringifiedUser = JSON.stringify({
      Name: 'testUser',
      Username: 'mockedUser',
      Id: '1245566',
      DisplayName: 'testUser',
    });
    const myStub = sinon.stub(PostsManager, 'GetPostsForUser').throws(new Error());
    const response = await request(server)
      .get('/PostsApi/posts/GetAllPostsForCurrentUser').send(stringifiedUser).expect(200);
    const stubCalledCorrectly = myStub.calledWith(notStringifiedUser.Id);
    expect(stubCalledCorrectly).toBe(true);
    expect(response.body.HasBeenSuccessful).toBe(false);
    expect(response.body.Content).toBe(null);
    expect(response.body.Errors).toBe('Api internal error-Manager');
    myStub.restore();
  });

  it('should hit GetSavedPostsForCurrentUSer and return 200 OK with some posts', async () => {
    const responseFromManager = [];
    const notStringifiedUser = {

      Name: 'testUser',
      Username: 'mockedUser',
      Id: '1245566',
      DisplayName: 'testUser',
      SavedArticles: [
        '11111111111',
        '22222222222',
      ],


    };
    const stringifiedReq = {

      CurrentArticles:[
        "11111111111",
        "22222222222"
      ]


    };
    responseFromManager.push(new Post({
      Content: 'test content',
      Title: 'test content',
      AuthorId: '12345678899899',
      AuthorName: 'test author name',
      IsVisible: false,
      _id: '22222222222',
    }),
    new Post({
      Content: 'test content 2',
      Title: 'test content 2',
      AuthorId: '12345678899899',
      _id: '11111111111',
      AuthorName: 'test author name 2',
      IsVisible: false,

    }));

    const myStub = sinon.stub(PostsManager, 'GetSavedPostsForCurrentUser').returns(responseFromManager);
    const response = await request(server)
      .post('/PostsApi/posts/GetSavedPostsForCurrentUSer').send(stringifiedReq).expect(200);
    const stubCalledCorrectly = myStub.calledWith(notStringifiedUser.SavedArticles);
    expect(stubCalledCorrectly).toBe(true);
    expect(response.body.HasBeenSuccessful).toBe(true);
    expect(response.body.Content[0].Content).toBe('test content');
    expect(response.body.Errors).toBe(null);
    myStub.restore();
  });

  it('should hit GetSavedPostsForCurrentUSer and return 200 OK with no posts', async () => {
    const notStringifiedUser = {
      CurrentArticles: [
        '22222222222',
        '11111111111',
      ],
    };
    const stringifiedUser = {
      CurrentArticles: [
        '22222222222',
        '11111111111',
      ],
    };
    const myStub = sinon.stub(PostsManager, 'GetSavedPostsForCurrentUser').returns(null);
    const response = await request(server)
      .post('/PostsApi/posts/GetSavedPostsForCurrentUSer').send(stringifiedUser).expect(200);
    const stubCalledCorrectly = myStub.calledWith(notStringifiedUser.CurrentArticles);
    expect(stubCalledCorrectly).toBe(true);
    expect(response.body.HasBeenSuccessful).toBe(false);
    expect(response.body.Content).toBe(null);
    expect(response.body.Errors).toBe('Api internal error-Manager');
    myStub.restore();
  });
  it('should hit GetPendingPosts and return 200 OK with some posts', async () => {
    const responseFromManager = [];
    const notStringifiedUser = {
      Name: 'testUser',
      Username: 'mockedUser',
      Id: '1245566',
      DisplayName: 'testUser',
      SavedArticles: [
        '22222222222',
        '11111111111',
      ],
    };
    const stringifiedUser = JSON.stringify({
      Name: 'testUser',
      Username: 'mockedUser',
      Id: '1245566',
      DisplayName: 'testUser',
      SavedArticles: [
        '22222222222',
        '11111111111',
      ],
    });
    responseFromManager.push(new Post({
      Content: 'test content',
      Title: 'test content',
      AuthorId: '12345678899899',
      AuthorName: 'test author name',
      IsVisible: false,
      _id: '22222222222',
    }),
    new Post({
      Content: 'test content 2',
      Title: 'test content 2',
      AuthorId: '12345678899899',
      _id: '11111111111',
      AuthorName: 'test author name 2',
      IsVisible: false,

    }));

    const myStub = sinon.stub(PostsManager, 'GetPendingPosts').returns(responseFromManager);
    const response = await request(server)
      .get('/PostsApi/posts/GetPendingPosts').expect(200);
    expect(response.body.HasBeenSuccessful).toBe(true);
    expect(response.body.Content.length).toBe(2);
    expect(response.body.Content[0].Content).toBe('test content');
    expect(response.body.Errors).toBe(null);
    myStub.restore();
  });
  it('should hit GetPendingPosts and return 200 OK failed request', async () => {
    const myStub = sinon.stub(PostsManager, 'GetPendingPosts').returns(null);
    const response = await request(server)
      .get('/PostsApi/posts/GetPendingPosts').expect(200);
    expect(response.body.HasBeenSuccessful).toBe(false);
    expect(response.body.Content).toBe(null);
    expect(response.body.Errors).toBe('Api internal error-Manager');
    myStub.restore();
  });

  it('should hit GetPendingPosts and return 200 OK with thrown error', async () => {
    const myStub = sinon.stub(PostsManager, 'GetPendingPosts').throws(new Error());
    const response = await request(server)
      .get('/PostsApi/posts/GetPendingPosts').expect(200);
    expect(response.body.HasBeenSuccessful).toBe(false);
    expect(response.body.Content).toBe(null);
    expect(response.body.Errors).toBe('Api internal error-Manager');
    myStub.restore();
  });


  it('should hit GetSavedPostsForCurrentUSer and return 200 OK error', async () => {
    const notStringifiedUser = {
      Name: 'testUser',
      Username: 'mockedUser',
      Id: '1245566',
      DisplayName: 'testUser',
      SavedArticles: [
        '22222222222',
        '11111111111',
      ],
    };
    const stringifiedUser = JSON.stringify({
      Name: 'testUser',
      Username: 'mockedUser',
      Id: '1245566',
      DisplayName: 'testUser',
      CurrentArticles: [
        '22222222222',
        '11111111111',
      ],
    });
    const myStub = sinon.stub(PostsManager, 'GetSavedPostsForCurrentUser').throws(new Error());
    const response = await request(server)
      .post('/PostsApi/posts/GetSavedPostsForCurrentUSer').send(stringifiedUser).expect(200);
    const stubCalledCorrectly = myStub.calledWith(notStringifiedUser.CurrentArticles);
    expect(stubCalledCorrectly).toBe(true);
    expect(response.body.HasBeenSuccessful).toBe(false);
    expect(response.body.Content).toBe(null);
    expect(response.body.Errors).toBe('Api internal error-Manager');
    myStub.restore();
  });

  it('should hit PinAPost and return 200 OK with successful request made', async () => {
    const Id = '123121331321312';
    const stringifiedReq = {
      Id: '123121331321312',
    };

    const myStub = sinon.stub(PostsManager, 'PinAPost').returns(true);
    const response = await request(server)
      .put('/PostsApi/posts/PinAPost').send(stringifiedReq).expect(200);
    const stubCalledCorrectly = myStub.calledWith(Id);
    expect(stubCalledCorrectly).toBe(true);
    expect(response.body.HasBeenSuccessful).toBe(true);
    expect(response.body.Errors).toBe(null);
    myStub.restore();
  });

  it('should hit PinAPost and return 200 OK with FAILED request made', async () => {
    const Id = '123121331321312';
    const stringifiedReq = {
      Id: '123121331321312',
    };

    const myStub = sinon.stub(PostsManager, 'PinAPost').returns(false);
    const response = await request(server)
      .put('/PostsApi/posts/PinAPost').send(stringifiedReq).expect(200);
    const stubCalledCorrectly = myStub.calledWith(Id);
    expect(stubCalledCorrectly).toBe(true);
    expect(response.body.HasBeenSuccessful).toBe(false);
    expect(response.body.Errors).toBe('Api internal error-Manager');
    myStub.restore();
  });

  it('should hit PinAPost and return 200 OK with throw error request made', async () => {
    const Id = '123121331321312';
    const stringifiedReq = {
      Id: '123121331321312',
    };

    const myStub = sinon.stub(PostsManager, 'PinAPost').throws(new Error());
    const response = await request(server)
      .put('/PostsApi/posts/PinAPost').send(stringifiedReq).expect(200);
    const stubCalledCorrectly = myStub.calledWith(Id);
    expect(stubCalledCorrectly).toBe(true);
    expect(response.body.HasBeenSuccessful).toBe(false);
    expect(response.body.Errors).toBe('Api internal error-Manager');
    myStub.restore();
  });


  it('should hit RateAPost and return 200 OK with successful request made', async () => {
    const expectedArgsToBeCalled = {
      postId: '12341231233',
      rating: '5',
    };
    const stringifiedReq = {
      postId: '12341231233',
      rating: '5',
    };

    const myStub = sinon.stub(PostsManager, 'RateAPost').returns(10);
    const response = await request(server)
      .put('/PostsApi/posts/RateAPost')
      .send(stringifiedReq)
      .expect(200);
    const stubCalledCorrectly = myStub.calledWith(expectedArgsToBeCalled.postId, expectedArgsToBeCalled.rating);
    expect(stubCalledCorrectly).toBe(true);
    expect(response.body.Content).toBe(10);
    expect(response.body.HasBeenSuccessful).toBe(true);
    expect(response.body.Errors).toBe(null);
    myStub.restore();
  });

  it('should hit RateAPost and return 200 OK with failed request made', async () => {
    const expectedArgsToBeCalled = {
      postId: '12341231233',
      rating: '5',
    };
    const stringifiedReq = {
      postId: '12341231233',
      rating: '5',
    };

    const myStub = sinon.stub(PostsManager, 'RateAPost').returns(null);
    const response = await request(server)
      .put('/PostsApi/posts/RateAPost').send(stringifiedReq).expect(200);
    const stubCalledCorrectly = myStub.calledWith(expectedArgsToBeCalled.postId, expectedArgsToBeCalled.rating);
    expect(stubCalledCorrectly).toBe(true);
    expect(response.body.Content).toBe(null);
    expect(response.body.HasBeenSuccessful).toBe(false);
    expect(response.body.Errors).toBe('Api internal error-Manager');
    myStub.restore();
  });

  it('should hit RateAPost and return 200 OK with throw Error request made', async () => {
    const expectedArgsToBeCalled = {
      postId: '12341231233',
      rating: '5',
    };
    const stringifiedReq = {
      postId: '12341231233',
      rating: '5',
    };

    const myStub = sinon.stub(PostsManager, 'RateAPost').throws(new Error());
    const response = await request(server)
      .put('/PostsApi/posts/RateAPost').send(stringifiedReq).expect(200);
    const stubCalledCorrectly = myStub.calledWith(expectedArgsToBeCalled.postId, expectedArgsToBeCalled.rating);
    expect(stubCalledCorrectly).toBe(true);
    expect(response.body.Content).toBe(null);
    expect(response.body.HasBeenSuccessful).toBe(false);
    expect(response.body.Errors).toBe('Api internal error-Manager');
    myStub.restore();
  });

  it('should hit ReviewAPost and return 200 OK with succesful request made', async () => {
    const Id = '22222222222';
    const IsVisible = true;
    const stringifiedReq = {
      Id: '22222222222',
      IsVisible: true,
    };
    const responseFromManager = [];
    responseFromManager.push(new Post({
      Content: 'test content',
      Title: 'test content',
      AuthorId: '12345678899899',
      AuthorName: 'test author name',
      IsVisible: true,
      ReviewedByAdmin: true,
      _id: '22222222222',
    }));

    const myStub = sinon.stub(PostsManager, 'ReviewedByAdmin').returns(responseFromManager);
    const response = await request(server)
      .put('/PostsApi/posts/ReviewAPost')
      .send(stringifiedReq)
      .expect(200);
    const stubCalledCorrectly = myStub.calledWith(Id, IsVisible);
    expect(stubCalledCorrectly).toBe(true);
    expect(response.body.HasBeenSuccessful).toBe(true);
    expect(response.body.Errors).toBe(null);
    myStub.restore();
  });

  it('should hit ReviewAPost and return 200 OK with failed request made', async () => {
    const expectedArgsToBeCalled = {
      Id: '22222222222',
      IsVisible: true,
    };
    const stringifiedReq = {
      Id: '22222222222',
      IsVisible: true,
    };

    const myStub = sinon.stub(PostsManager, 'ReviewedByAdmin').returns(null);
    const response = await request(server)
      .put('/PostsApi/posts/ReviewAPost').send(stringifiedReq).expect(200);
    const stubCalledCorrectly = myStub.calledWith(expectedArgsToBeCalled.Id, expectedArgsToBeCalled.IsVisible);
    expect(stubCalledCorrectly).toBe(true);
    expect(response.body.HasBeenSuccessful).toBe(false);
    expect(response.body.Errors).toBe('Api internal error-Manager');
    myStub.restore();
  });

  it('should hit ReviewAPost and return 200 OK with thrown error', async () => {
    const myStub = sinon.stub(PostsManager, 'ReviewedByAdmin').throws(new Error());
    const response = await request(server)
      .put('/PostsApi/posts/ReviewAPost').send().expect(200);
    expect(response.body.HasBeenSuccessful).toBe(false);
    expect(response.body.Errors).toBe('Api internal error-Manager');
    myStub.restore();
  });

  it('should hit UpdatePost and return 200 OK with succesful request made', async () => {
    const postToUpdate = {
      Content: 'testContent',
      Title: 'Bblah title',
      Id: '1231321',
    };


    const stringifiedReq = {
      Content: 'testContent',
      Title: 'Bblah title',
      _id: '1231321',
    };

    const myStub = sinon.stub(PostsManager, 'UpdatePost').returns(true);
    const response = await request(server)
      .post('/PostsApi/posts/UpdatePost')
      .send(stringifiedReq)
      .expect(200);
    const stubCalledCorrectly = myStub.calledWith(postToUpdate);
    expect(stubCalledCorrectly).toBe(true);
    expect(response.body.HasBeenSuccessful).toBe(true);
    expect(response.body.Errors).toBe(null);
    myStub.restore();
  });

  it('should hit UpdatePost and return 200 OK with failed request made', async () => {
    const postToUpdate = {
      Content: 'testContent',
      Title: 'Bblah title',
    };


    const stringifiedReq = {
      WRONGREQ: 'Blach',
    };

    const myStub = sinon.stub(PostsManager, 'UpdatePost').returns(false);
    const response = await request(server)
      .post('/PostsApi/posts/UpdatePost')
      .send(stringifiedReq)
      .expect(200);
    expect(response.body.HasBeenSuccessful).toBe(false);
    expect(response.body.Errors).toBe('Api internal error-Manager');
    myStub.restore();
  });

  it('should hit UpdatePost and return 200 OK with throw error request made', async () => {
    const postToUpdate = {
      Content: 'testContent',
      Title: 'Bblah title',
    };


    const stringifiedReq = {
      WRONGREQ: 'Blach',
    };

    const myStub = sinon.stub(PostsManager, 'UpdatePost').throws(new Error());
    const response = await request(server)
      .post('/PostsApi/posts/UpdatePost')
      .send(stringifiedReq)
      .expect(200);
    expect(response.body.HasBeenSuccessful).toBe(false);
    expect(response.body.Errors).toBe('Api internal error-Manager');
    myStub.restore();
  });


  it('should hit AddCommentToPost and return 200 OK with error throw when request made', async () => {
    const stringifiedReq = {
      WRONGREQ: 'HAHAHAHAHAHHA',
    };

    const myStub = sinon.stub(PostsManager, 'AddCommentToPost').throws(new Error());
    const response = await request(server)
      .put('/PostsApi/posts/AddCommentToPost')
      .send(stringifiedReq)
      .expect(200);
    expect(response.body.HasBeenSuccessful).toBe(false);
    expect(response.body.Errors).toBe('Api internal error-Manager');
    myStub.restore();
  });
  it('should hit AddCommentToPost and return 200 OK with failed request made', async () => {
    const stringifiedReq = {
      WRONGREQ: 'HAHAHAHAHAHHA',
    };

    const myStub = sinon.stub(PostsManager, 'AddCommentToPost').returns(null);
    const response = await request(server)
      .put('/PostsApi/posts/AddCommentToPost')
      .send(stringifiedReq)
      .expect(200);
    expect(response.body.HasBeenSuccessful).toBe(false);
    expect(response.body.Errors).toBe('Api internal error-Manager');
    myStub.restore();
  });
  it('should hit AddCommentToPost and return 200 OK with succesful request made', async () => {
    const expected = {
      postId: '22222222222',
      User: {
        DisplayName: 'God admin',
      },
      Comment: 'I am a god admin, i own this api',
    };


    const stringifiedReq = {
      postId: '22222222222',
      User: {
        DisplayName: 'God admin',
      },
      Comment: 'I am a god admin, i own this api',
    };
    const responseFromManager = [];
    responseFromManager.push(new Post({
      Content: 'test content',
      Title: 'test content',
      AuthorId: '12345678899899',
      AuthorName: 'test author name',
      Comment: [
        {
          Author: 'God admin',
          content: 'I am a god admin, i own this api',
        },
      ],
      IsVisible: true,
      ReviewedByAdmin: true,
      _id: '22222222222',
    }));
    const myStub = sinon.stub(PostsManager, 'AddCommentToPost').returns(responseFromManager);
    const response = await request(server)
      .put('/PostsApi/posts/AddCommentToPost')
      .send(stringifiedReq)
      .expect(200);
    const stubCalledCorrectly = myStub.calledWith(expected.postId.trim(), expected.User.DisplayName.trim(), expected.Comment.trim());
    expect(stubCalledCorrectly).toBe(true);
    expect(response.body.HasBeenSuccessful).toBe(true);
    expect(response.body.Errors).toBe(null);
    myStub.restore();
  });

  it('should hit SavePost and return 200 OK with succesful request made', async () => {
    const dto = new PostDto();
    dto.AuthorName = 'TestAuthorName';
    dto.Content = 'testContent';
    dto.AuthorId = '213211232';
    dto.Images = [];
    dto.Comments = [];
    dto.Title = 'Bblah title';

    const expected = {
      formattedPost: dto,
      succedded: true,
    };


    const stringifiedReq = {
      Content: 'testContent',
      Title: 'Bblah title',
      User: {
        Id: '213211232',
        Username: 'TestAuthorName',
      },
    };

    const myStub = sinon.stub(PostsManager, 'SavePost').returns(true);
    const response = await request(server)
      .post('/PostsApi/posts/SavePost')
      .send(stringifiedReq)
      .expect(200);
    const stubCalledCorrectly = myStub.calledWith(expected.formattedPost);
    expect(stubCalledCorrectly).toBe(true);
    expect(response.body.HasBeenSuccessful).toBe(true);
    expect(response.body.Errors).toBe(null);
    myStub.restore();
  });
  it('should hit SavePost and return 200 OK with failed post format', async () => {
    const myStub = sinon.stub(PostsManager, 'SavePost').returns(false);
    const response = await request(server)
      .post('/PostsApi/posts/SavePost')
      .send()
      .expect(200);
    expect(response.body.HasBeenSuccessful).toBe(false);
    expect(response.body.Errors).toBe('Post not valid for submission');
    myStub.restore();
  });

  it('should hit SavePost and return 200 OK with failed request made', async () => {
    const stringifiedReq = {
      Content: 'testContent',
      Title: 'Bblah title',
      User: {
        Id: '213211232',
        Username: 'TestAuthorName',
      },
    };
    const myStub = sinon.stub(PostsManager, 'SavePost').returns(false);
    const response = await request(server)
      .post('/PostsApi/posts/SavePost')
      .send(stringifiedReq)
      .expect(200);
    expect(response.body.HasBeenSuccessful).toBe(false);
    expect(response.body.Errors).toBe('Api internal error-Manager');
    myStub.restore();
  });
  it('should hit SavePost and return 200 OK with throw request made', async () => {
    const stringifiedReq = {
      Content: 'testContent',
      Title: 'Bblah title',
      User: {
        Id: '213211232',
        Username: 'TestAuthorName',
      },
    };
    const myStub = sinon.stub(PostsManager, 'SavePost').throws(new Error());
    const response = await request(server)
      .post('/PostsApi/posts/SavePost')
      .send(stringifiedReq)
      .expect(200);
    expect(response.body.HasBeenSuccessful).toBe(false);
    expect(response.body.Errors).toBe('Api internal error-Manager');
    myStub.restore();
  });
});
