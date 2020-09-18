<template>
  <div class="mesgs">
  <div class="msg_history">
    <div class="incoming_msg">
      <div class="incoming_msg_img"> <img src="https://ptetutorials.com/images/user-profile.png" alt="sunil"> </div>
      <div class="received_msg">
        <div class="received_withd_msg">
          <p>Test which is a new approach to have all
            solutions</p>
          <span class="time_date"> 11:01 AM    |    June 9</span></div>
      </div>
    </div>
    <div class="outgoing_msg">
      <div class="sent_msg">
        <p>Test which is a new approach to have all
          solutions</p>
        <span class="time_date"> 11:01 AM    |    June 9</span> </div>
    </div>
    <div class="incoming_msg">
      <div class="incoming_msg_img"> <img src="https://ptetutorials.com/images/user-profile.png" alt="sunil"> </div>
      <div class="received_msg">
        <div class="received_withd_msg">
          <p>Test, which is a new approach to have</p>
          <span class="time_date"> 11:01 AM    |    Yesterday</span></div>
      </div>
    </div>
    <div class="outgoing_msg">
      <div class="sent_msg">
        <p>Apollo University, Delhi, India Test</p>
        <span class="time_date"> 11:01 AM    |    Today</span> </div>
    </div>
    <div class="incoming_msg">
      <div class="incoming_msg_img"> <img src="https://ptetutorials.com/images/user-profile.png" alt="sunil"> </div>
      <div class="received_msg">
        <div class="received_withd_msg">
          <p>We work directly with our designers and suppliers,
            and sell direct to you, which means quality, exclusive
            products, at a price anyone can afford.</p>
          <span class="time_date"> 11:01 AM    |    Today</span></div>
      </div>
    </div>
  </div>
  <div class="type_msg">
    <div class="input_msg_write">
      <input type="text" class="write_msg" placeholder="Type a message" />
      <button class="msg_send_btn" type="button"><i class="fa fa-paper-plane-o" aria-hidden="true"></i></button>
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
      messageList: [],
      message:null
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
    },
    sendMessage: function () {
      let parameters = JSON.stringify({
        from: localStorage.userId,
        to: this.contactId,
        content: this.message
      })
      axios.post(`${this.apiUrl}api/message/writemessage`, parameters, {
        headers: {
          'Content-Type': 'application/json; charset=utf-8'
        }})
        .then(function (response) {
          if (response.data.success) {
          }
        })
        .catch(function (error) {
          console.log(error)
        })
    }
  },
  created: function () {
    let self = this
    EventBus.$on('show-contact-messages', function(contactId){
      self.contactId = contactId
      self.getMessages();
    })
    this.$socket.on('ReceiveMessage', (message) => { 
      if(message.to === localStorage.userId || message.for === localStorage.userId)
        self.getMessages()
    });
  },
  sockets: {
    MessageSent (message){
    }
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

<style>
</style>
