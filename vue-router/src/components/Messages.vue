<template>
  <div class="content">
    <div class="messages">
      <ul v-for="message in messageList" v-bind:key="message.id">
        <li v-if="message.sent_by_me" class="sent">
          <img v-bind:src="require(`../assets/${message.profile_picture}`)" alt="" />
          <p>{{message.message}}</p>
        </li>
        <li v-else class="replies">
          <img v-bind:src="require(`../assets/${message.profile_picture}`)" alt="" />
          <p>{{message.message}}</p>
        </li>
      </ul>
    </div>
    <div class="message-input">
      <div class="wrap">
      <input type="text" placeholder="Write your message..." />
      <i class="fa fa-paperclip attachment" aria-hidden="true"></i>
      <button class="submit"><i class="fa fa-paper-plane" aria-hidden="true"></i></button>
      </div>
    </div>
  </div>
</template>

<script>
import mixins from '../mixins.js'
import axios from 'axios'
import { EventBus } from '../event-bus.js'

export default {
  name: 'home',
  mixins: [mixins],
  data () {
    return {
      contactId: null,
      messageList: []
    }
  },
  methods: {
    getMessages: function () {
      let self = this;
      if(this.contactId != null) {
        axios.get(`${this.apiUrl}api/message/getmessages?user_id=${localStorage.userId}&contact_id=${this.contactId}`)
          .then(function (response) {
            if (response.data.success) {
              self.messageList = response.data.values              
            } else {
            }
          })
          .then(function() {
            StyleMyMessageBubbles()
          }).catch(error => {
            console.log(error)
        })
      }
    }
  },
  created: function () {
    let self = this
    EventBus.$on('show-contact-messages', function(contactId){
      self.contactId = contactId
      self.getMessages();
    })    
  },
  mounted: function () {
  }
}
function StyleMyMessageBubbles () {
    let messageBubbles = document.getElementsByClassName('sent')
    for (let i = 0; i < messageBubbles.length; i++) {
      messageBubbles[i].style.backgroundColor = localStorage.primaryColor
      messageBubbles[i].style.borderRadius = '20px'
    }
}
</script>

<style scoped>
#frame .content .messages ul li.sent img {
  margin: 6px 8px 0 0;
}
#frame .content .messages ul li.sent p {
  color: #f5f5f5;
}
</style>
