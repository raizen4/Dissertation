/* eslint-disable no-unused-vars */
/* eslint-disable prefer-destructuring */
const mockingoose = require('mockingoose').default;
const moongose = require('mongoose');
const sinon = require('sinon');
const Post = require('../../DbSchemas/PostsSchema');
const PostsManager = require('../../manager/PostsManager');


describe('Posts db tests', () => {
  beforeEach(() => {
    mockingoose.resetAll();
  });

  describe('Post manager tests', async () => {
    it('should return true from the manager if the post is valid', async () => {
      const doc = new Post({
        Title: 'test title',
        Content: 'test content',
        AuthorId: '12345678899899',
        AuthorName: 'test author name',

      });

      mockingoose.Post.toReturn(doc, 'save');
      const result = await PostsManager.SavePost(doc);
      expect(result).toBe(true);
    });

    it('should fail to save a new post as there is already a post with the same data there', async () => {
      const doc = new Post({
        Title: 'test title',
        Content: 'test content',
        AuthorId: '12345678899899',
        AuthorName: 'test author name',
      });

      const myStub = sinon.stub(Post.prototype, 'save').returns(null);
      const managerResult = await PostsManager.SavePost(doc);
      expect(managerResult).toBe(false);
      myStub.restore();
    });

    it('should throw an error when trying to save', async () => {
      const doc = new Post({
        Title: 'test title',
        Content: 'test content',
        AuthorId: '12345678899899',
        AuthorName: 'test author name',
      });

      const myStub = sinon.stub(Post.prototype, 'save').throws(new Error('blach'));
      const managerResult = await PostsManager.SavePost(doc);
      expect(managerResult).toBe(false);
      myStub.restore();
    });

    it('should throw an error if the required fields are  not present', () => {
      const doc = new Post({
        AuthorId: '12345678899899',
        AuthorName: 'test author name',

      });
      try {
        doc.validate();
      } catch (err) {
        expect(err).toBeDefined();
      }
    });

    it('should get all the posts', async () => {
      const doc = [];
      doc.push(new Post({
        Content: 'test content',
        Title: 'test content',
        AuthorId: '12345678899899',
        AuthorName: 'test author name',

      }),
      new Post({
        Content: 'test content 2',
        Title: 'test content 2',
        AuthorId: '12345678899899',
        AuthorName: 'test author name 2',

      }));
      const myStub = sinon.stub(moongose.Model, 'find').returns(doc);
      const managerResult = await PostsManager.GetAllPosts();
      expect(managerResult.length).toBe(2);
      myStub.restore();
    });

    it('should get an error while trying to retrieve posts', async () => {
      mockingoose.Post.toReturn(new Error(), 'find');
      const managerResult = await PostsManager.GetAllPosts();
      expect(managerResult).toBe(null);
    });

    it('should get all the posts which are pending reviews', async () => {
      const doc = [];
      doc.push(new Post({
        Content: 'test content',
        Title: 'test content',
        AuthorId: '12345678899899',
        AuthorName: 'test author name',
        IsVisible: false,


      }),
      new Post({
        Content: 'test content 2',
        Title: 'test content 2',
        AuthorId: '12345678899899',
        AuthorName: 'test author name 2',
        IsVisible: false,

      }));

      mockingoose.Post.toReturn(doc, 'find');
      const managerResult = await PostsManager.GetPendingPosts();
      expect(managerResult.length).toBe(2);
    });

    it('should get an error while trying to retrieve pending posts', async () => {
      mockingoose.Post.toReturn(new Error('Testing catch from this method in manager'), 'find');
      const managerResult = await PostsManager.GetPendingPosts();
      expect(managerResult).toBe(null);
    });

    it('should get NO POSTS when trying to retrieve pending posts', async () => {
      mockingoose.Post.toReturn(null, 'find');
      const managerResult = await PostsManager.GetPendingPosts();
      expect(managerResult).toBe(null);
    });

    it('should get all the posts for current logged user', async () => {
      const doc = [];
      doc.push(new Post({
        Content: 'test content',
        Title: 'test content',
        AuthorId: '12345678899899',
        AuthorName: 'test author name',
        IsVisible: false,


      }),
      new Post({
        Content: 'test content 2',
        Title: 'test content 2',
        AuthorId: '12345678899899',
        AuthorName: 'test author name 2',
        IsVisible: false,

      }));
      const myStub = sinon.stub(moongose.Model, 'find').returns(doc);
      const managerResult = await PostsManager.GetPostsForUser('12345678899899'); 
      myStub.restore();
      expect(managerResult.length).toBe(2);
      expect(managerResult[0].AuthorName).toBe('test author name');
    });


    it('should get an error while trying to retrieve posts for user', async () => {
      mockingoose.Post.toReturn(new Error('Testing catch from this method in manager'), 'find');
      const managerResult = await PostsManager.GetPostsForUser();
      expect(managerResult).toBe(null);
    });

    it('should get No results while trying to retrieve posts for user', async () => {
      const myStub = sinon.stub(moongose.Model, 'find').returns(null);
      const managerResult = await PostsManager.GetPostsForUser('Nope'); 
      myStub.restore();
      expect(managerResult).toBe(null);
     
    });

    it('should get all the saved posts for current logged user', async () => {
      const doc = [];
      doc.push(new Post({
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

      mockingoose.Post.toReturn(doc, 'find');
      const arrayOfSavedArticles = [];
      arrayOfSavedArticles.push('11111111111');
      arrayOfSavedArticles.push('22222222222');
      const managerResult = await PostsManager.GetSavedPostsForCurrentUser(arrayOfSavedArticles);
      expect(managerResult.length).toBe(2);
      expect(managerResult[0].AuthorName).toBe('test author name');
      expect(managerResult[1].AuthorName).toBe('test author name 2');
    });

    it('should get an error while trying to  get all the saved posts for current logged user', async () => {
      mockingoose.Post.toReturn(new Error('Testing catch from this method in manager'), 'find');
      const managerResult = await PostsManager.GetSavedPostsForCurrentUser();
      expect(managerResult).toBe(null);
    });

    it('should update a post with a new Content ', async () => {
      const docInDb = new Post({
        Content: 'test content',
        Title: 'test content',
        AuthorId: '12345678899899',
        AuthorName: 'test author name',
        _id: '1111111111',
      });
      const expectedUpdatedDoc = new Post({
        Content: 'test content UPDATED BY UPDATE METHOD',
        Title: 'test content',
        AuthorId: '12345678899899',
        AuthorName: 'test author name',
        _id: '1111111111',
      });
      const newPostDetails = {
        Content: 'test content UPDATED BY UPDATE METHOD',
        Title: 'test content',
        id: '1111111111',
      };
      const myStub = sinon.stub(moongose.Model, 'findByIdAndUpdate').returns(expectedUpdatedDoc);
      const managerResult = await PostsManager.UpdatePost(newPostDetails);
      expect(managerResult).toBe(true);
      myStub.restore();
    });

    it('should not find the post or something went wrong in the db method ', async () => {
      const docInDb = new Post({
        Content: 'test content',
        Title: 'test content',
        AuthorId: '12345678899899',
        AuthorName: 'test author name',
        _id: '1111111111',
      });
      const expectedUpdatedDoc = new Post({
        Content: 'test content UPDATED BY UPDATE METHOD',
        Title: 'test content',
        AuthorId: '12345678899899',
        AuthorName: 'test author name',
        _id: '1111111111',
      });
      const newPostDetails = {
        Content: 'test content UPDATED BY UPDATE METHOD',
        Title: 'test content',
        id: '1111111111',
      };
      const myStub = sinon.stub(moongose.Model, 'findByIdAndUpdate').returns(null);
      const managerResult = await PostsManager.UpdatePost(newPostDetails);
      expect(managerResult).toBe(false);
      myStub.restore();
    });

    it('should get an error whilst trying to update ', async () => {
      const docInDb = new Post({
        Content: 'test content UPDATED BY UPDATE METHOD',
        Title: 'test content',
        AuthorId: '12345678899899',
        AuthorName: 'test author name',
        _id: '1111111111',
      });
      const expectedUpdatedDoc = new Post({
        Content: 'test content UPDATED BY UPDATE METHOD',
        Title: 'test content',
        AuthorId: '12345678899899',
        AuthorName: 'test author name',
        _id: '1111111111',
      });

      const myStub = sinon.stub(moongose.Model, 'findByIdAndUpdate').returns(new Error('This is to test the catch'));
      const managerResult = await PostsManager.UpdatePost(null);
      expect(managerResult).toBe(false);
      myStub.restore();
    });

    it('should get an error whilst trying to update ', async () => {
      const docInDb = new Post({
        Content: 'test content UPDATED BY UPDATE METHOD',
        Title: 'test content',
        AuthorId: '12345678899899',
        AuthorName: 'test author name',
        _id: '1111111111',
      });
      const expectedUpdatedDoc = new Post({
        Content: 'test content UPDATED BY UPDATE METHOD',
        Title: 'test content',
        AuthorId: '12345678899899',
        AuthorName: 'test author name',
        _id: '1111111111',
      });

      const myStub = sinon.stub(moongose.Model, 'findByIdAndUpdate').returns(new Error('This is to test the catch'));
      const managerResult = await PostsManager.UpdatePost(null);
      expect(managerResult).toBe(false);
      myStub.restore();
    });
    it('should update a post with a new Comment ', async () => {
      const expectedUpdatedDoc = new Post({
        Content: 'test content UPDATED BY UPDATE METHOD',
        Title: 'test content',
        AuthorId: '12345678899899',
        AuthorName: 'test author name',
        _id: '1111111111',
        Comments: [
          {
            _id: '12321312',
            Author: 'Bogdan',
            content: 'test',
          },

        ],
      });
      const myStub = sinon.stub(moongose.Model, 'findByIdAndUpdate').returns(expectedUpdatedDoc);
      const managerResult = await PostsManager.AddCommentToPost('1111111111', 'Bogdan', 'Test');
      expect(managerResult.Comments[0].Author).toBe('Bogdan');
      myStub.restore();
    });

    it('should not find a post to update its comments with the id given and returns null ', async () => {
      const myStub = sinon.stub(moongose.Model, 'findByIdAndUpdate').returns(null);
      const managerResult = await PostsManager.AddCommentToPost('22222222222', 'Bogdan', 'Test');
      expect(managerResult).toBe(null);
      myStub.restore();
    });

    it('should throw an erro while adding a comment ', async () => {
      const myStub = sinon.stub(moongose.Model, 'findByIdAndUpdate').throws(new Error('Error to test catch'));
      const managerResult = await PostsManager.AddCommentToPost('22222222222', 'Bogdan', 'Test');
      expect(managerResult).toBe(null);
      myStub.restore();
    });

    it('should pin a post ', async () => {
      const expectedUpdatedDoc = new Post({
        Content: 'test content UPDATED BY UPDATE METHOD',
        Title: 'test content',
        AuthorId: '12345678899899',
        AuthorName: 'test author name',
        _id: '1111111111',
        IsFeatured: true,
      });
      const myStub = sinon.stub(moongose.Model, 'findByIdAndUpdate').returns(expectedUpdatedDoc);
      const managerResult = await PostsManager.PinAPost('1111111111');
      expect(managerResult).toBe(true);
      myStub.restore();
    });

    it('should not pin a post as it cant find its id in the db ', async () => {
      const myStub = sinon.stub(moongose.Model, 'findByIdAndUpdate').returns(null);
      const managerResult = await PostsManager.PinAPost('222222222');
      expect(managerResult).toBe(false);
      myStub.restore();
    });

    it('should throw an error when it tries to pin a post ', async () => {
      const myStub = sinon.stub(moongose.Model, 'findByIdAndUpdate').throws(new Error('Blah'));
      const managerResult = await PostsManager.PinAPost('222222222');
      expect(managerResult).toBe(false);
      myStub.restore();
    });

    it('should be reviewed by admin ', async () => {
      const expectedUpdatedDoc = new Post({
        Content: 'test content UPDATED BY UPDATE METHOD',
        Title: 'test content',
        AuthorId: '12345678899899',
        AuthorName: 'test author name',
        _id: '1111111111',
        ReviewedByAdmin: true,
        IsVisible: true,
      });
      const myStub = sinon.stub(moongose.Model, 'findByIdAndUpdate').returns(expectedUpdatedDoc);
      const managerResult = await PostsManager.ReviewedByAdmin('1111111111', true);
      expect(managerResult.IsVisible).toBe(true);
      expect(managerResult.ReviewedByAdmin).toBe(true);
      myStub.restore();
    });
    it('should fail to find the post for ReviewedByAdmin method', async () => {
      const myStub = sinon.stub(moongose.Model, 'findByIdAndUpdate').returns(null);
      const managerResult = await PostsManager.ReviewedByAdmin('222222222', true);
      expect(managerResult).toBe(null);
      myStub.restore();
    });
    it('should  throw and error for ReviewedByAdmin method', async () => {
      const myStub = sinon.stub(moongose.Model, 'findByIdAndUpdate').throws(new Error('Blah'));
      const managerResult = await PostsManager.ReviewedByAdmin('222222222', true);
      expect(managerResult).toBe(null);
      myStub.restore();
    });

    it('should add a new rating for a post ', async () => {
      const expectedValues = {
        Content: 'test content UPDATED BY UPDATE METHOD',
        Title: 'test content',
        AuthorId: '12345678899899',
        AuthorName: 'test author name',
        _id: '1111111111',
        TimesReviewed: 2,
        Rating: '8',
      };

      const myStub = sinon.stub(moongose.Model, 'findByIdAndUpdate').returns(expectedValues);
      const managerResult = await PostsManager.RateAPost('1111111111', '4');
      expect(managerResult.Rating).toBe(expectedValues.Rating);
      expect(managerResult.TimesReviewed).toBe(expectedValues.TimesReviewed);
      myStub.restore();
    });
    it('should should fail to add a new rating for a past ', async () => {
      const myStub = sinon.stub(moongose.Model, 'findByIdAndUpdate').returns(null);
      const managerResult = await PostsManager.RateAPost('22222222', '4');
      expect(managerResult).toBe(null);
      myStub.restore();
    });
    it('should should fail to add a new rating for a past ', async () => {
      const myStub = sinon.stub(moongose.Model, 'findByIdAndUpdate').returns(null);
      const managerResult = await PostsManager.RateAPost('22222222', '4');
      expect(managerResult).toBe(null);
      myStub.restore();
    });

    it('should should throw an error for a new rating for a past ', async () => {
      const myStub = sinon.stub(moongose.Model, 'findByIdAndUpdate').throws(new Error('blach'));
      const managerResult = await PostsManager.RateAPost('22222222', '4');
      expect(managerResult).toBe(null);
      myStub.restore();
    });
  });
});
